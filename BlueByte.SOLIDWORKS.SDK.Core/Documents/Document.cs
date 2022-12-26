using BlueByte.SOLIDWORKS.SDK.Core.CustomProperties;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
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
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyChanged;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyAdded;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyDeleted;
        public event EventHandler GotLoaded;

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

        private bool isLoaded;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; NotifyPropertyChanged(nameof(IsLoaded)); }
        }

        public CustomProperties.ICustomPropertyManager CustomPropertyManager { get; set; }


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
                            partDoc.DestroyNotify2 += document_DestroyNotify2;
                            partDoc.AddCustomPropertyNotify += document_AddCustomPropertyNotify;
                            partDoc.DeleteCustomPropertyNotify += document_DeleteCustomPropertyNotify;
                            partDoc.ChangeCustomPropertyNotify += document_ChangeCustomPropertyNotify;
                        }
                    }
                    break;

                case swDocumentTypes_e.swDocASSEMBLY:
                    {
                        if (assemblyDoc != null)
                        {
                            assemblyDoc.DestroyNotify2 += assemblyDoc_DestroyNotify2;
                            assemblyDoc.AddCustomPropertyNotify += document_AddCustomPropertyNotify;
                            assemblyDoc.DeleteCustomPropertyNotify += document_DeleteCustomPropertyNotify;
                            assemblyDoc.ChangeCustomPropertyNotify += document_ChangeCustomPropertyNotify;
                            assemblyDoc.FileReloadNotify += assemblyDoc_FileReloadNotify;
                        }
                    }
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    {
                        if (drawingDoc != null)
                        {
                            drawingDoc.DestroyNotify2 += DrawingDoc_DestroyNotify2; ;
                            drawingDoc.AddCustomPropertyNotify += document_AddCustomPropertyNotify;
                            drawingDoc.DeleteCustomPropertyNotify += document_DeleteCustomPropertyNotify; ;
                            drawingDoc.ChangeCustomPropertyNotify += document_ChangeCustomPropertyNotify; ;
                            drawingDoc.FileReloadNotify += DrawingDoc_FileReloadNotify; ;
                        }
                    }
                    break;

                default:
                    break;
            }
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
        private int DrawingDoc_FileReloadNotify()
        {
            throw new NotImplementedException();
        }

        private int document_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType)
        {
            var eventArgs = CustomPropertyChangedEventArgs.New(ChangeType.Change, this as IDocument, propName, Configuration, oldValue, NewValue);
            CustomPropertyChanged?.Invoke(this, eventArgs);

            if (eventArgs.Handled)
            {
                this.DettachEventHandlers();
                CustomPropertyManager.Set(this, propName, NewValue == null ? String.Empty : NewValue.ToLower(), Configuration);
                this.AttachEventHandlers();

            }

            return 0;
        }

        private int document_DeleteCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            var eventArgs = CustomPropertyChangedEventArgs.New(ChangeType.Delete, this as IDocument, propName, Configuration);
            CustomPropertyDeleted?.Invoke(this, eventArgs);

            if (eventArgs.Handled)
            {
                this.DettachEventHandlers();
                CustomPropertyManager.AddSafe(this, propName, Value == null ? String.Empty : Value.ToLower(), (swCustomInfoType_e)valueType, Configuration);
                this.AttachEventHandlers();
            }
            return 0;
        }

        private int document_AddCustomPropertyNotify(string propName, string Configuration, string Value, int valueType)
        {
            var eventArgs = CustomPropertyChangedEventArgs.New(ChangeType.Add, this as IDocument, propName, Configuration, Value, string.Empty);
            CustomPropertyAdded?.Invoke(this, eventArgs);
            if (eventArgs.Handled)
            {
                this.DettachEventHandlers();
                CustomPropertyManager.Delete(this, propName, Configuration);
                this.AttachEventHandlers();
            }

            return 0;
        }

        private int assemblyDoc_DestroyNotify2(int DestroyType)
        {
            if (GotClosed != null)
                GotClosed(this, (swDestroyNotifyType_e)DestroyType);

            return 0;
        }

       

        private int document_DestroyNotify2(int DestroyType)
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

            switch (DocumentType)
            {
                case swDocumentTypes_e.swDocPART:
                    {
                        if (partDoc != null)
                        {
                            partDoc.AddCustomPropertyNotify -= document_AddCustomPropertyNotify;
                            partDoc.DeleteCustomPropertyNotify -= document_DeleteCustomPropertyNotify;
                            partDoc.ChangeCustomPropertyNotify -= document_ChangeCustomPropertyNotify;
                            partDoc.DestroyNotify2 -= document_DestroyNotify2;
                        }
                    }
                    break;

                case swDocumentTypes_e.swDocASSEMBLY:
                    {
                        if (assemblyDoc != null)
                        {
                            assemblyDoc.AddCustomPropertyNotify -= document_AddCustomPropertyNotify;
                            assemblyDoc.DeleteCustomPropertyNotify -= document_DeleteCustomPropertyNotify;
                            assemblyDoc.ChangeCustomPropertyNotify -= document_ChangeCustomPropertyNotify;
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
                            drawingDoc.AddCustomPropertyNotify -= document_AddCustomPropertyNotify;
                            drawingDoc.DeleteCustomPropertyNotify -= document_DeleteCustomPropertyNotify; ;
                            drawingDoc.ChangeCustomPropertyNotify -= document_ChangeCustomPropertyNotify; ;
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
            // flush all events

            GotClosed = null;
            CustomPropertyAdded = null;
            CustomPropertyDeleted = null;
            CustomPropertyChanged = null;

            this.PropertyChanged -= Document_PropertyChanged;


            base.Flush();


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
            
        }
    }
}