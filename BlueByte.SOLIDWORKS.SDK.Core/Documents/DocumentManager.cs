using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    public class DocumentManager : IDocumentManager, IDisposable
    {
        #region Public Events

        public event EventHandler<Tuple<IDocument, swDestroyNotifyType_e>> DocumentGotClosed;

        public event EventHandler<IDocument> DocumentGotCreated;

        public event EventHandler<IDocument> DocumentGotOpened;

        #endregion

        #region Public Properties

        public ObservableCollection<IDocument> Documents { get; set; } = new ObservableCollection<IDocument>();

        internal SldWorks SwApp { get; }

        #endregion

        #region Public Constructors

        public DocumentManager(ISOLIDWORKSApplication app)
        {
            SwApp = app.UnSafeObject;
        }

        #endregion

        #region Private Methods

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

            return 0;
        }

        private int SwApp_FileOpenNotify2(string FileName)
        {
            var model = SwApp.IGetOpenDocumentByName2(FileName);

            var retAdd = GetDocumentFromUnsafeObject(model);
            if (DocumentGotOpened != null)
                DocumentGotOpened(this, retAdd.Item1);

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
            _document.GotClosed += document_GotClosed;
            //DocumentManagerExtension.Add(Documents, _document);
            return _document;
        }

        public void AttachEventHandlers()
        {
            SwApp.FileOpenNotify2 += SwApp_FileOpenNotify2;
            SwApp.FileNewNotify2 += SwApp_FileNewNotify2;
        }

        public void DeattachEventHandlers()
        {
            SwApp.FileOpenNotify2 -= SwApp_FileOpenNotify2;
            SwApp.FileNewNotify2 -= SwApp_FileNewNotify2;
        }

        public void Dispose()
        {
            DeattachEventHandlers();
        }

        public Dictionary<IDocument, DocumentAddOperationRet_e> LoadExistingDocuments()
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
                    IAssembly assembly = new Document(model, model.GetPathName());
                    assembly.DettachEventHandlers();
                    assembly.AttachEventHandlers();
                    assembly.GotClosed += document_GotClosed;
                    //DocumentManagerExtension.Add(Documents, assembly);
                    retValue = DocumentAddOperationRet_e.Added;
                    retDocument = assembly;
                }
                else
                {
                    var newDocument = Document.New(model);
                    newDocument.DettachEventHandlers();
                    newDocument.AttachEventHandlers();
                    newDocument.GotClosed += document_GotClosed;
                    //DocumentManagerExtension.Add(Documents, newDocument);
                    retValue = DocumentAddOperationRet_e.Added;
                    retDocument = newDocument;
                }
            }

            return new Tuple<IDocument, DocumentAddOperationRet_e>(retDocument, retValue);
        }

        #endregion
    }
}


 