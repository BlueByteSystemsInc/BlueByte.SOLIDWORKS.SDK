using BlueByte.SOLIDWORKS.SDK.Core.CustomProperties;
using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
using BlueByte.SOLIDWORKS.SDK.Exceptions;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{

    public static class ModelDoc2Helper
    {

        /// <summary>
        /// Gets the title2.
        /// </summary>
        /// <param name="modelDoc2">The model doc2.</param>
        /// <returns></returns>
        public static string GetTitle2(this ModelDoc2 modelDoc2)
        {

            var fileName = modelDoc2.GetTitle();
            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);

            var type = (swDocumentTypes_e)modelDoc2.GetType();

            switch (type)
            {
                case swDocumentTypes_e.swDocNONE:
                    return fileName;
                case swDocumentTypes_e.swDocPART:
                    return $"{fileNameWithoutExtension}.sldprt";
                case swDocumentTypes_e.swDocASSEMBLY:
                    return $"{fileNameWithoutExtension}.sldasm";
                case swDocumentTypes_e.swDocDRAWING:
                    return $"{fileNameWithoutExtension}.slddrw";
                case swDocumentTypes_e.swDocSDM:
                    return fileNameWithoutExtension;
                case swDocumentTypes_e.swDocLAYOUT:
                    return fileNameWithoutExtension;
                case swDocumentTypes_e.swDocIMPORTED_PART:
                    return $"{fileNameWithoutExtension}.sldprt";
                case swDocumentTypes_e.swDocIMPORTED_ASSEMBLY:
                    return $"{fileNameWithoutExtension}.sldasm";
                default:
                    break;
            }


            return fileName;

        }
    }


    internal class Document : SOLIDWORKSObject, IDocument
    {

        #region fields 

        internal PartDoc partDoc =  default(PartDoc);
        internal AssemblyDoc assemblyDoc =  default(AssemblyDoc);
        internal DrawingDoc drawingDoc = default(DrawingDoc);


        #endregion


        #region events 

        public event EventHandler<swDestroyNotifyType_e> GotClosed;
        public event EventHandler<SaveEventArgs> BeforeSavedAs;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyChanged;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyAdded;
        public event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyDeleted;
        public event EventHandler GotLoaded;
        public event EventHandler Rebuilt;
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
                var obj = this.UnSafeObject as ModelDoc2;

                if (obj != null)
                    this.IsVisible = obj.Visible;
                this.PathName = obj.GetPathName();
                this.FileName = System.IO.Path.GetFileName(obj.GetPathName());

            }
        }

        public string ActiveConfigurationName { get;   set; }

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
                FileName = model.GetTitle2();
                PathName = model.GetPathName();
                switch (DocumentType)
                {
                    case swDocumentTypes_e.swDocPART:
                        partDoc = model as PartDoc;
                        ActiveConfigurationName = (model.GetActiveConfiguration() as Configuration).Name;
                        break;

                    case swDocumentTypes_e.swDocASSEMBLY:
                        assemblyDoc = model as AssemblyDoc;
                        ActiveConfigurationName = (model.GetActiveConfiguration() as Configuration).Name;
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


        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.FileName))
                return this.FileName;

            return base.ToString();
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
                            partDoc.FileSaveAsNotify2 += document_FileSaveAsNotify2;
                            partDoc.AddCustomPropertyNotify += document_AddCustomPropertyNotify;
                            partDoc.DeleteCustomPropertyNotify += document_DeleteCustomPropertyNotify;
                            partDoc.ChangeCustomPropertyNotify += document_ChangeCustomPropertyNotify;
                            partDoc.ActiveConfigChangePostNotify += document_ActiveConfigChangePostNotify; ;

                            partDoc.FeatureManagerTreeRebuildNotify += document_FeatureManagerTreeRebuildNotify;

                        }
                    }
                    break;

                case swDocumentTypes_e.swDocASSEMBLY:
                    {
                        if (assemblyDoc != null)
                        {
                            assemblyDoc.DestroyNotify2 += document_DestroyNotify2;
                            assemblyDoc.FileSaveAsNotify2 += document_FileSaveAsNotify2;
                            assemblyDoc.AddCustomPropertyNotify += document_AddCustomPropertyNotify;
                            assemblyDoc.DeleteCustomPropertyNotify += document_DeleteCustomPropertyNotify;
                            assemblyDoc.ChangeCustomPropertyNotify += document_ChangeCustomPropertyNotify;
                            assemblyDoc.FileReloadNotify += document_FileReloadNotify;
                            assemblyDoc.ActiveConfigChangePostNotify += document_ActiveConfigChangePostNotify;
                            assemblyDoc.FeatureManagerTreeRebuildNotify += document_FeatureManagerTreeRebuildNotify;
                        }
                    }
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    {
                        if (drawingDoc != null)
                        {
                            drawingDoc.DestroyNotify2 += document_DestroyNotify2;
                            drawingDoc.FileSaveAsNotify2 += document_FileSaveAsNotify2;
                            drawingDoc.AddCustomPropertyNotify += document_AddCustomPropertyNotify;
                            drawingDoc.DeleteCustomPropertyNotify += document_DeleteCustomPropertyNotify; 
                            drawingDoc.ChangeCustomPropertyNotify += document_ChangeCustomPropertyNotify; 
                            drawingDoc.FileReloadNotify += document_FileReloadNotify;
                            drawingDoc.FeatureManagerTreeRebuildNotify += document_FeatureManagerTreeRebuildNotify;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private int document_FeatureManagerTreeRebuildNotify()
        {
            if (this.Rebuilt != null)
                this.Rebuilt.Invoke(this, EventArgs.Empty);


            return 0;
        }

        /// <summary>
        /// Shows the specified configuration.
        /// </summary>
        /// <param name="configurationName"></param>
        /// <exception cref="BlueByte.SOLIDWORKS.SDK.Exceptions.SOLIDWORKSSDKException">
        /// Cannot show configuration {configurationName} because document {this.FileName} is not loaded.
        /// or
        /// Configuration {configurationName} does exit in {this.FileName}.
        /// or
        /// Unsafe object failed to show configuration.
        /// </exception>
        public void ShowConfiguration(string configurationName)
        {
            if (this.IsLoaded == false)
                throw new SOLIDWORKSSDKException($"Cannot show configuration {configurationName} because document {this.FileName} is not loaded.");

            var model = UnSafeObject as ModelDoc2;

            if (model == null) return;

            
            var configurationNames = model.GetConfigurationNames() as string[];

            if (string.IsNullOrWhiteSpace(configurationNames.FirstOrDefault(x=> x.Equals(configurationName))))
                    throw new SOLIDWORKSSDKException($"Configuration {configurationName} does exit in {this.FileName}.");


            var showConfigurationRet = model.ShowConfiguration2(configurationName);

            if (showConfigurationRet == false)
                throw new SOLIDWORKSSDKException($"Unsafe object failed to show configuration.");

        }


        private int document_ActiveConfigChangePostNotify()
        {
            var model = UnSafeObject as ModelDoc2;

            if (model == null) return 0;

            var activeConfiguration = (model.GetActiveConfiguration() as Configuration).Name;

            ActiveConfigurationName = activeConfiguration;  

            return 0;
        }

   

        private int document_FileSaveAsNotify2(string FileName)
        {
            var eventArgs = SaveEventArgs.New(this, FileName);

            if (BeforeSavedAs != null)
                BeforeSavedAs.Invoke(this, eventArgs);
            
            if (eventArgs.Handled)
                return (int)HRESULT.S_False;

            return (int)HRESULT.S_OK;

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



                GotLoaded?.Invoke(this, EventArgs.Empty);


            }
            else
                IsLoaded = false;
        }



        public SaveRet[] Save(FileExtension_e extensions = FileExtension_e.Default)
        {

            var l = new List<SaveRet>();

            if (IsLoaded == false)
                throw new SOLIDWORKSSDKException($"{FileName} is not loaded in memory.");

            if (UnSafeObject == null)
                throw new SOLIDWORKSSDKException($"{FileName} is not loaded in memory.", new NullReferenceException(nameof(UnSafeObject)));

            var model = UnSafeObject as ModelDoc2;

            var swModExt = model.Extension;

            bool status = false;

            var filePath = model.GetPathName();

             int errors = 0;
            int warnings = 0;


            if (string.IsNullOrWhiteSpace(model.GetPathName()))
                throw new SOLIDWORKSSDKException($"{FileName} has not been saved before. Please use SaveAs before using this method.");



            var members = Enum.GetValues(typeof(FileExtension_e));

            foreach (var member in members)
            {
                var e = (FileExtension_e)member;

                if (extensions.HasFlag(e) == false)
                    continue;


                switch (e)
                {
                    case FileExtension_e.Default:
                    case FileExtension_e.sldprt:
                    case FileExtension_e.sldasm:
                    case FileExtension_e.slddrw:
                        if (EnumHelper.Equals(e, this))
                        {
                            status = model.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, errors, warnings);
                            l.Add(new SaveRet() { Extension = e, Errors = errors, Warnings = warnings, Success = status });
                        }
                        continue;
                    case FileExtension_e.stp:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".stp");
                        break;
                    case FileExtension_e.x_t:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".x_t");
                        break;
                    case FileExtension_e.pdf:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".pdf");
                        break;
                    case FileExtension_e.iges:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".iges");
                        break;
                    case FileExtension_e.igs:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".igs");
                        break;
                    case FileExtension_e._3mf:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".3mf");

                        break;
                    case FileExtension_e.dxf:
                        filePath = System.IO.Path.ChangeExtension(filePath, ".dxf");
                        break;
                    default:
                        break;
                }

                status = swModExt.SaveAs(filePath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errors, ref warnings);
                l.Add(new SaveRet() { Extension = e, Errors = errors, Warnings = warnings, Success = status });


            }


            return l.ToArray();

        }

        private int document_FileReloadNotify()
        {

            var application = Globals.Application.As<SldWorks>();

            if (string.IsNullOrWhiteSpace(this.FileName))
                return 0;

            var modelDoc = application.GetOpenDocumentByName(this.FileName);

            if (modelDoc != null)
            this.Load(modelDoc);

            return 0;
        }
       

        private int document_ChangeCustomPropertyNotify(string propName, string Configuration, string oldValue, string NewValue, int valueType)
        {
            var eventArgs = CustomPropertyChangedEventArgs.New(ChangeType.Change, this as IDocument, propName, Configuration, oldValue, NewValue);
            CustomPropertyChanged?.Invoke(this, eventArgs);

            if (eventArgs.Handled)
            {
                this.DettachEventHandlers();
                CustomPropertyManager.Set(this, propName, NewValue == null ? String.Empty : oldValue.ToLower(), Configuration);
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
            return System.IO.Path.GetFileName(filename).Equals(System.IO.Path.GetFileName(f), StringComparison.OrdinalIgnoreCase);
        }

        public virtual void DettachEventHandlers()
        {

            switch (DocumentType)
            {
                case swDocumentTypes_e.swDocPART:
                    {
                        if (partDoc != null)
                        {
                            partDoc.FileSaveAsNotify2 -= document_FileSaveAsNotify2;
                            partDoc.AddCustomPropertyNotify -= document_AddCustomPropertyNotify;
                            partDoc.DeleteCustomPropertyNotify -= document_DeleteCustomPropertyNotify;
                            partDoc.ChangeCustomPropertyNotify -= document_ChangeCustomPropertyNotify;
                            partDoc.DestroyNotify2 -= document_DestroyNotify2;
                            partDoc.ActiveConfigChangePostNotify -= document_ActiveConfigChangePostNotify;
                            partDoc.FeatureManagerTreeRebuildNotify-= document_FeatureManagerTreeRebuildNotify;
                             
                        }
                    }
                    break;

                case swDocumentTypes_e.swDocASSEMBLY:
                    {
                        if (assemblyDoc != null)
                        {
                            assemblyDoc.FileSaveAsNotify2 -= document_FileSaveAsNotify2;

                            assemblyDoc.AddCustomPropertyNotify -= document_AddCustomPropertyNotify;
                            assemblyDoc.DeleteCustomPropertyNotify -= document_DeleteCustomPropertyNotify;
                            assemblyDoc.ChangeCustomPropertyNotify -= document_ChangeCustomPropertyNotify;
                            assemblyDoc.DestroyNotify2 -= document_DestroyNotify2;
                            assemblyDoc.FileReloadNotify -= document_FileReloadNotify;
                            assemblyDoc.ActiveConfigChangePostNotify -= document_ActiveConfigChangePostNotify;
                            assemblyDoc.FeatureManagerTreeRebuildNotify -= document_FeatureManagerTreeRebuildNotify;
                        }
                    }
                    break;
                case swDocumentTypes_e.swDocDRAWING:
                    {
                        if (drawingDoc != null)
                        {
                            drawingDoc.FileSaveAsNotify2 -= document_FileSaveAsNotify2;
                            drawingDoc.DestroyNotify2 -= document_DestroyNotify2;
                            drawingDoc.AddCustomPropertyNotify -= document_AddCustomPropertyNotify;
                            drawingDoc.DeleteCustomPropertyNotify -= document_DeleteCustomPropertyNotify; 
                            drawingDoc.ChangeCustomPropertyNotify -= document_ChangeCustomPropertyNotify; 
                            drawingDoc.FileReloadNotify -= document_FileReloadNotify;
                            drawingDoc.FeatureManagerTreeRebuildNotify -= document_FeatureManagerTreeRebuildNotify;
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
            BeforeSavedAs = null;

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

            try
            {
                if (UnSafeObject != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(UnSafeObject);

               

            }
            catch (Exception)
            {
                 
            }
            finally
            {
                UnSafeObject = null;
            }
            
            IsLoaded = false;




        }


        private void Document_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
    }




}





