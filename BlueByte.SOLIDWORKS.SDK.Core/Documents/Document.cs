using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{

    internal class Document : SOLIDWORKSObject, IDocument
    {

        #region fields 

        PartDoc partDoc =  default(PartDoc);
        AssemblyDoc assemblyDoc =  default(AssemblyDoc);
        DrawingDoc drawingDoc = default(DrawingDoc);


        #endregion


        #region events 

        public event EventHandler<swDestroyNotifyType_e> GotClosed;

        #endregion 

        /// <summary>
        /// Gets or sets the solidworks object.
        /// </summary>
        /// <value>
        /// The solidworks object.
        /// </value>
        
        
        public void Refresh()
        {
            if (this.UnSafeObject != null && this.IsLoaded)
            {
                this.IsVisible = (this.UnSafeObject as ModelDoc2).Visible;
                this.PathName = (this.UnSafeObject as ModelDoc2).GetPathName();
                this.FileName = System.IO.Path.GetFileName((this.UnSafeObject as ModelDoc2).GetPathName());

            }
        }

        public bool IsVisible { get; internal set; }
        
        public string FileName { get; private set; }
        
        public string PathName { get; private set; }

        public swDocumentTypes_e DocumentType { get; private set; }
        
        public bool IsLoaded { get; private set; }

        


        public static Document New(ModelDoc2 model, string fullFileName = "", bool isRoot = false)
        {
            var instance = default(Document);
            
            instance = new Document(model, fullFileName, isRoot);

            return instance;
        }

        internal Document(ModelDoc2 model, string fullFileName = "", bool isRoot = false)
        {
          

            
            if (model == null)
            {
                var extension = System.IO.Path.GetExtension(fullFileName);
                
                DocumentType = extension.ToLower().Contains("sldprt") ? swDocumentTypes_e.swDocPART : swDocumentTypes_e.swDocASSEMBLY;
                
                if (extension.ToLower().Contains("slddrw"))
                    DocumentType = swDocumentTypes_e.swDocDRAWING;


                FileName = System.IO.Path.GetFileName(fullFileName);
                PathName = fullFileName;
                IsLoaded = false;
            }
            else
            {
                base.UnSafeObject = model;
                DocumentType = (swDocumentTypes_e)model.GetType();
                FileName = model.GetTitle();
                PathName = model.GetPathName();
                switch (DocumentType)
                {
                    case swDocumentTypes_e.swDocPART:
                        partDoc = model as PartDoc;
                        break;

                    case swDocumentTypes_e.swDocASSEMBLY:
                        assemblyDoc = model as AssemblyDoc;
                        break;

                    case swDocumentTypes_e.swDocDRAWING:
                        drawingDoc = model as DrawingDoc;
                        break;
                }

                IsVisible = model.Visible;
                IsLoaded = true;
            }

            this.PropertyChanged += Document_PropertyChanged;

            #region add property service 

            // todo:

            #endregion
        }


        public virtual void AttachEventHandlers()
        {
            switch (DocumentType)
            {
                case swDocumentTypes_e.swDocPART:
                    {
                        if (partDoc != null)
                        {
                            partDoc.DestroyNotify2 += PartDoc_DestroyNotify2;
                            partDoc.AddCustomPropertyNotify += PartDoc_AddCustomPropertyNotify;
                            partDoc.DeleteCustomPropertyNotify += PartDoc_DeleteCustomPropertyNotify;
                            partDoc.ChangeCustomPropertyNotify += PartDoc_ChangeCustomPropertyNotify;
                        }
                    }
                    break;

                case swDocumentTypes_e.swDocASSEMBLY:
                    {
                        if (assemblyDoc != null)
                        {
                            assemblyDoc.DestroyNotify2 += assemblyDoc_DestroyNotify2;
                            assemblyDoc.AddCustomPropertyNotify += assemblyDoc_AddCustomPropertyNotify;
                            assemblyDoc.DeleteCustomPropertyNotify += assemblyDoc_DeleteCustomPropertyNotify;
                            assemblyDoc.ChangeCustomPropertyNotify += assemblyDoc_ChangeCustomPropertyNotify;
                            assemblyDoc.FileReloadNotify += assemblyDoc_FileReloadNotify;
                        }
                    }
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    {
                        if (drawingDoc != null)
                        {
                            drawingDoc.DestroyNotify2 += DrawingDoc_DestroyNotify2; ;
                            drawingDoc.AddCustomPropertyNotify += DrawingDoc_AddCustomPropertyNotify;
                            drawingDoc.DeleteCustomPropertyNotify += DrawingDoc_DeleteCustomPropertyNotify; ;
                            drawingDoc.ChangeCustomPropertyNotify += DrawingDoc_ChangeCustomPropertyNotify; ;
                            drawingDoc.FileReloadNotify += DrawingDoc_FileReloadNotify; ;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private int DrawingDoc_FileReloadNotify()
        {
            throw new NotImplementedException();
        }

        private int DrawingDoc_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType)
        {
            throw new NotImplementedException();
        }

        private int DrawingDoc_DeleteCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            throw new NotImplementedException();
        }

        private int DrawingDoc_AddCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            throw new NotImplementedException();
        }

        private int DrawingDoc_DestroyNotify2(int DestroyType)
        {
            if (GotClosed != null)
                GotClosed(this, (swDestroyNotifyType_e)DestroyType);

            return 0;
        }

        public void Load(object UnsafeObject)
        {
            var model = UnsafeObject as ModelDoc2; 
            UnSafeObject = model;


            if (UnSafeObject != null)
            {
                DocumentType = (swDocumentTypes_e)model.GetType();

                switch (DocumentType)
                {
                    case swDocumentTypes_e.swDocPART:
                        partDoc = model as PartDoc;
                        break;

                    case swDocumentTypes_e.swDocASSEMBLY:
                        assemblyDoc = model as AssemblyDoc;
                        break;
                    case swDocumentTypes_e.swDocDRAWING:
                        drawingDoc = model as DrawingDoc;
                        break;

                    default:
                        break;
                }

                IsLoaded = true;


                DettachEventHandlers();
                AttachEventHandlers();

                #region rettach property service 

                // todo: 

                #endregion 



            }
            else
                IsLoaded = false;
        }


        private int assemblyDoc_FileReloadNotify()
        {
            throw new NotImplementedException();
        }

        private int assemblyDoc_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType)
        {
            throw new NotImplementedException();
        }

        private int assemblyDoc_DeleteCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            throw new NotImplementedException();
        }

        private int assemblyDoc_AddCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            throw new NotImplementedException();
        }

        private int assemblyDoc_DestroyNotify2(int DestroyType)
        {
            if (GotClosed != null)
                GotClosed(this, (swDestroyNotifyType_e)DestroyType);

            return 0;
        }

        private int PartDoc_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType)
        {
            throw new NotImplementedException();
        }

        private int PartDoc_DeleteCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            throw new NotImplementedException();
        }

        private int PartDoc_AddCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            throw new NotImplementedException();
        }

        private int PartDoc_DestroyNotify2(int DestroyType)
        {
            if (GotClosed != null)
                GotClosed(this, (swDestroyNotifyType_e)DestroyType);

            return 0;
        }

        public bool Equals(IDocument doc)
        {
            var f = this.FileName;
            if (System.IO.Path.HasExtension(f))
                return doc.FileName.Equals(f);

            return System.IO.Path.GetFileNameWithoutExtension(doc.FileName).Equals(System.IO.Path.GetFileNameWithoutExtension(f));
        }

        public bool Equals(string filename)
        {
            var f = this.FileName;
            if (System.IO.Path.HasExtension(f))
                return filename.Equals(f);

            return System.IO.Path.GetFileNameWithoutExtension(filename).Equals(System.IO.Path.GetFileNameWithoutExtension(f));
        }

        public virtual void DettachEventHandlers()
        {
            GotClosed = null;

            switch (DocumentType)
            {
                case swDocumentTypes_e.swDocPART:
                    {
                        if (partDoc != null)
                        {
                            partDoc.AddCustomPropertyNotify -= PartDoc_AddCustomPropertyNotify;
                            partDoc.DeleteCustomPropertyNotify -= PartDoc_DeleteCustomPropertyNotify;
                            partDoc.ChangeCustomPropertyNotify -= PartDoc_ChangeCustomPropertyNotify;
                            partDoc.DestroyNotify2 -= PartDoc_DestroyNotify2;
                        }
                    }
                    break;

                case swDocumentTypes_e.swDocASSEMBLY:
                    {
                        if (assemblyDoc != null)
                        {
                            assemblyDoc.AddCustomPropertyNotify -= assemblyDoc_AddCustomPropertyNotify;
                            assemblyDoc.DeleteCustomPropertyNotify -= assemblyDoc_DeleteCustomPropertyNotify;
                            assemblyDoc.ChangeCustomPropertyNotify -= assemblyDoc_ChangeCustomPropertyNotify;
                            assemblyDoc.DestroyNotify2 -= assemblyDoc_DestroyNotify2;
                            assemblyDoc.FileReloadNotify -= assemblyDoc_FileReloadNotify;
                        }
                    }
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    {
                        if (drawingDoc != null)
                        {
                            drawingDoc.DestroyNotify2 -= DrawingDoc_DestroyNotify2; ;
                            drawingDoc.AddCustomPropertyNotify -= DrawingDoc_AddCustomPropertyNotify;
                            drawingDoc.DeleteCustomPropertyNotify -= DrawingDoc_DeleteCustomPropertyNotify; ;
                            drawingDoc.ChangeCustomPropertyNotify -= DrawingDoc_ChangeCustomPropertyNotify; ;
                            drawingDoc.FileReloadNotify -= DrawingDoc_FileReloadNotify; ;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual void Dispose()
        {
            this.PropertyChanged -= Document_PropertyChanged;

            DettachEventHandlers();

            if (partDoc != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(partDoc);
            partDoc = null;

            if (assemblyDoc != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(assemblyDoc);

            assemblyDoc = null;

            if (drawingDoc != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(drawingDoc);

            drawingDoc = null;

            if (UnSafeObject != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(UnSafeObject);

            UnSafeObject = null;

            IsLoaded = false;




        }


        private void Document_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}