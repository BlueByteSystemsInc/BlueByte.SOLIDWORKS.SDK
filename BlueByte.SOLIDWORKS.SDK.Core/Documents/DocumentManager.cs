using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    public class DocumentManager : IDocumentManager, IDisposable , INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Public Events


        public event EventHandler<Tuple<IDocument, swDestroyNotifyType_e>> DocumentGotClosed;

        public event EventHandler<IDocument> DocumentGotCreated;

        public event EventHandler<IDocument> DocumentGotOpened;

        public event EventHandler<SaveEventArgs> DocumentAboutToBeSavedAs;

        public event EventHandler<IDocument> ActiveDocumentChanged;

        #endregion

        #region Public Properties


        private IDocument activeDoc;

        public IDocument ActiveDocument
        {
            get { return activeDoc; }
            set { activeDoc = value; NotifyPropertyChanged(nameof(ActiveDocument)); }
        }

        ObservableCollection<IDocument> Documents { get; set; } = new ObservableCollection<IDocument>();

        internal SldWorks SwApp { get; }

        #endregion

        #region Public Constructors

        public DocumentManager(ISOLIDWORKSApplication app)
        {
            SwApp = app.UnSafeObject;
      
            // there may be a better place to put this - i dont know where since we have decided not to use the service locator
            Globals.DocumentManager = this;
        }

        #endregion

        #region Private Methods


        public IDocument[] GetDocuments()
        {
            return Documents.ToArray();
        }


        private void document_GotClosed(object sender, swDestroyNotifyType_e e)
        {
            switch (e)
            {
                case swDestroyNotifyType_e.swDestroyNotifyDestroy:
                    {
                        var document = sender as Document;
                        document.Dispose();
                        Documents.Remove(document);
                    }
                    break;

                case swDestroyNotifyType_e.swDestroyNotifyHidden:
                    {
                        // document in memory just hidden
                        var document = sender as Document;
                        document.IsVisible = false;
                    }
                    break;

                default:
                    break;
            }

            if (DocumentGotClosed != null)
                DocumentGotClosed(this, new Tuple<IDocument, swDestroyNotifyType_e>(sender as IDocument, e));
        }

        private int SwApp_FileNewNotify2(object NewDoc, int DocType, string TemplateName)
        {
            var documentType = (swDocumentTypes_e)DocType;
            if (documentType == swDocumentTypes_e.swDocASSEMBLY || documentType == swDocumentTypes_e.swDocPART)
            {
                var retAdd = GetDocumentFromUnsafeObject(NewDoc as ModelDoc2);
                if (DocumentGotCreated != null)
                    DocumentGotCreated(this, retAdd.Item1);
            }

            // refresh active document
            SwApp_ActiveModelDocChangeNotify();

            return 0;
        }

        private int SwApp_FileOpenNotify2(string FileName)
        {
            var model = SwApp.IGetOpenDocumentByName2(FileName);

            var retAdd = GetDocumentFromUnsafeObject(model);
            if (DocumentGotOpened != null)
                DocumentGotOpened(this, retAdd.Item1);


            // refresh active document
            SwApp_ActiveModelDocChangeNotify();

            return 0;
        }

        #endregion

        #region Public Methods

        public IDocument AddUnloadedDocument(string fileName)
        {

            

            var _document = Document.New(null, fileName);
            if (_document.DocumentType == swDocumentTypes_e.swDocASSEMBLY)
                _document = new Document(null, fileName);

            if (_document.IsLoaded)
                throw new Exception("Attempted to add a loaded document");

            foreach (var document in Documents)
            {
                if (_document.FileName.ToLower() == document.FileName.ToLower())
                    return document;
            }

            _document.DettachEventHandlers();
            _document.AttachEventHandlers();

            _document.GotClosed -= document_GotClosed;
            _document.GotClosed += document_GotClosed;
            Documents.Add(_document);
            return _document;
        }

        public void AttachEventHandlers()
        {
            SwApp.ActiveModelDocChangeNotify += SwApp_ActiveModelDocChangeNotify; ;
            SwApp.FileOpenNotify2 += SwApp_FileOpenNotify2;
            SwApp.FileNewNotify2 += SwApp_FileNewNotify2;
        }

        private int SwApp_ActiveModelDocChangeNotify()
        {
            var modelDoc = SwApp.ActiveDoc as ModelDoc;

            if (modelDoc == null)
            {
                ActiveDocument = null;
                if (ActiveDocumentChanged != null)
                    ActiveDocumentChanged.Invoke(this, null);
                return 0;
            }
            
            var doc = this.Documents.ToArray().FirstOrDefault(x => x.Equals(modelDoc.GetTitle()));
            this.ActiveDocument = doc;

            if (ActiveDocumentChanged != null)
                ActiveDocumentChanged.Invoke(this, doc);


            return 0;
        }

        public void DettachEventHandlers()
        {
            SwApp.ActiveModelDocChangeNotify -= SwApp_ActiveModelDocChangeNotify;
            SwApp.FileOpenNotify2 -= SwApp_FileOpenNotify2;
            SwApp.FileNewNotify2 -= SwApp_FileNewNotify2;


            // remove internal events
            DocumentGotClosed = null;
            DocumentGotOpened = null;
            DocumentGotCreated = null;
            DocumentAboutToBeSavedAs = null;
        }

        public void Dispose()
        {
            DettachEventHandlers();


            foreach (var document in this.Documents)
                document.Dispose();

            this.Documents.Clear();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void InitializeWithPreloadedDocuments()
        {
            GetExistingDocuments();


            // get active document 
            SwApp_ActiveModelDocChangeNotify();
        }



        
        private Dictionary<IDocument, DocumentAddOperationRet_e> GetExistingDocuments()
        {
            var retValue = new Dictionary<IDocument, DocumentAddOperationRet_e>();
            var iteratingModel = SwApp.GetFirstDocument() as ModelDoc2;

            if (iteratingModel != null)
            {
                while (iteratingModel != null)
                {
                    if (iteratingModel.GetType() == (int)swDocumentTypes_e.swDocPART ||
                        iteratingModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                    {
                        var ret = GetDocumentFromUnsafeObject(iteratingModel);

                        if (DocumentGotOpened != null)
                            DocumentGotOpened(this, ret.Item1);

                        retValue.Add(ret.Item1, ret.Item2);
                    }

                    iteratingModel = iteratingModel.GetNext() as ModelDoc2;
                }
            }

            return retValue;
        }





        public Tuple<IDocument, DocumentAddOperationRet_e> GetDocumentFromUnsafeObject(object unsafeObject)
        {
            var model = unsafeObject as ModelDoc2;

            if (model == null)
                throw new ArgumentException($"Failed to get convert {nameof(unsafeObject)} to {nameof(ModelDoc2)}");

            var retValue = DocumentAddOperationRet_e.Nothing;
            var retDocument = default(IDocument);
            var addNewDocument = true;

            foreach (var document in Documents)
            {
                var Document = document as Document;

                if (model.GetTitle().ToLower() == Document.FileName.ToLower())
                {
                    if (Document.IsLoaded == false)
                    {
                        // document is loaded - used by suppressed components
                        Document.Load(model);
                        Document.DettachEventHandlers();
                        Document.AttachEventHandlers();
                        Document.GotClosed += document_GotClosed;
                        Document.BeforeSavedAs += document_BeforeSavedAs;
                        retValue = DocumentAddOperationRet_e.ReloadedInMemory;
                        addNewDocument = false;
                        retDocument = Document;
                        break;
                    }
                    else
                    {
                        // nothing to do here: document exists and is loaded.
                        retValue = DocumentAddOperationRet_e.Nothing;
                        addNewDocument = false;
                        retDocument = Document;
                        // update visibility
                        retDocument.Refresh();
                        break;
                    }
                }
            }

            if (addNewDocument)
            {
                if (model.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                {
                    IAssembly assembly = new Assembly(model, model.GetPathName(), true);
                    assembly.DettachEventHandlers();
                    assembly.AttachEventHandlers();
                    assembly.GotClosed += document_GotClosed;
                    assembly.BeforeSavedAs += document_BeforeSavedAs;
                    Documents.Add(assembly);
                    retValue = DocumentAddOperationRet_e.Added;
                    retDocument = assembly;
                    var configuration = model.GetActiveConfiguration() as Configuration;
                    assembly.Initialize(configuration.Name);
                }
                else
                {
                    var newDocument = Document.New(model);
                    newDocument.DettachEventHandlers();
                    newDocument.AttachEventHandlers();
                    newDocument.GotClosed += document_GotClosed;
                    newDocument.BeforeSavedAs += document_BeforeSavedAs;
                    Documents.Add(newDocument);
                    retValue = DocumentAddOperationRet_e.Added;
                    retDocument = newDocument;
                }
            }

            return new Tuple<IDocument, DocumentAddOperationRet_e>(retDocument, retValue);
        }

        private void document_BeforeSavedAs(object sender, SaveEventArgs e)
        {
            DocumentAboutToBeSavedAs?.Invoke(this, e);
        }

        #endregion
    }


    
}


 