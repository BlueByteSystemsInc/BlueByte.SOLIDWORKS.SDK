using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
using SolidWorks.Interop.sldworks;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.CustomProperties
{
    public class CustomPropertyManager : IDisposable
    {
        

        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyChanged;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyAdded;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyDeleted;


        public CustomPropertyManager(IDocumentManager documentManager)
        {
            documentManager.DocumentGotCreated += DocumentManager_DocumentGotCreated;
            documentManager.DocumentGotOpened += DocumentManager_DocumentGotOpened;
        }

        private void DocumentManager_DocumentGotOpened(object sender, IDocument e)
        {

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
          
            e.CustomPropertyAdded += Document_CustomPropertyAdded;
            e.CustomPropertyChanged += Document_CustomPropertyChanged;
            e.CustomPropertyDeleted += Document_CustomPropertyDeleted;
        }

        public void Dispose()
        {

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
