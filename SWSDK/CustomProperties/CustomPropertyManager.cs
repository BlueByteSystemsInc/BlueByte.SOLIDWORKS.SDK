using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
using SolidWorks.Interop.sldworks;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.CustomProperties
{
    /// <summary>
    /// Custom property manager. For internal use. Please use <see cref="ICustomPropertyManager"/>
    /// </summary>
    /// <seealso cref="BlueByte.SOLIDWORKS.SDK.Core.CustomProperties.ICustomPropertyManager" />
    /// <seealso cref="System.IDisposable" />
    public class CustomPropertyManager : ICustomPropertyManager, IDisposable
    {
        

        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyChanged;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyAdded;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyDeleted;


        public IDocumentManager DocumentManager { get; set; }

        public CustomPropertyManager(IDocumentManager documentManager)
        {

            this.DocumentManager = documentManager;
        }

        public void Initialize()
        {
            DocumentManager.DocumentGotCreated += DocumentManager_DocumentGotCreated;
            DocumentManager.DocumentGotOpened += DocumentManager_DocumentGotOpened;

            var openedDocuments = DocumentManager.GetDocuments();

            foreach (var document in openedDocuments)
            {
                document.CustomPropertyManager = this;
                document.CustomPropertyAdded += Document_CustomPropertyAdded;
                document.CustomPropertyChanged += Document_CustomPropertyChanged;
                document.CustomPropertyDeleted += Document_CustomPropertyDeleted;
                document.PropertyChanged += Document_PropertyChanged;

            }
        }

        private void Document_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(IDocument.IsLoaded)))
            {
                var document = e as IDocument;
                document.CustomPropertyManager = this;
            }
        }

        private void DocumentManager_DocumentGotOpened(object sender, IDocument e)
        {
            e.CustomPropertyManager = this;
            e.CustomPropertyAdded += Document_CustomPropertyAdded;
            e.CustomPropertyChanged += Document_CustomPropertyChanged;
            e.CustomPropertyDeleted += Document_CustomPropertyDeleted;



        }

        private void Document_CustomPropertyDeleted(object sender, CustomPropertyChangedEventArgs e)
        {
            CustomPropertyDeleted?.Invoke(this, e);
        }

        private void Document_CustomPropertyChanged(object sender, CustomPropertyChangedEventArgs e)
        {
            CustomPropertyChanged?.Invoke(this, e);
        }

        private void Document_CustomPropertyAdded(object sender, CustomPropertyChangedEventArgs e)
        {
            CustomPropertyAdded?.Invoke(this, e);
        }

        private void DocumentManager_DocumentGotCreated(object sender, IDocument e)
        {
            e.CustomPropertyManager = this;
            e.CustomPropertyAdded += Document_CustomPropertyAdded;
            e.CustomPropertyChanged += Document_CustomPropertyChanged;
            e.CustomPropertyDeleted += Document_CustomPropertyDeleted;
            e.PropertyChanged += Document_PropertyChanged;
        }

        public void Dispose()
        {

        }

        public void AddSafe(IDocument doc, string propertyName, object value, SolidWorks.Interop.swconst.swCustomInfoType_e dataType = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText, string configurationName = "")
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var modeldoc = doc.UnSafeObject as ModelDoc2;

            if (modeldoc == null)
                throw new Exception("Could get the api model object to set the property.");

            modeldoc.Extension.CustomPropertyManager[configurationName].Add3(propertyName, (int)dataType, value.ToString(), (int)SolidWorks.Interop.swconst.swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

        }
        public void Set(IDocument doc, string propertyName, object value, string configurationName = "")
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var modeldoc = doc.UnSafeObject as ModelDoc2;

            if (modeldoc == null)
                throw new Exception("Could get the api model object to set the property.");

            modeldoc.Extension.CustomPropertyManager[configurationName].Set2(propertyName, value.ToString());

        }
        public void Delete(IDocument doc, string propertyName, string configurationName = "")
        {
           
            var modeldoc = doc.UnSafeObject as ModelDoc2;

            if (modeldoc == null)
                throw new Exception("Could get the api model object to set the property.");

            modeldoc.Extension.CustomPropertyManager[configurationName].Delete(propertyName);

        }
    }
}
