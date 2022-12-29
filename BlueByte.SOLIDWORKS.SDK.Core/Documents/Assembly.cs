using BlueByte.SOLIDWORKS.SDK.Core.Documents.Components;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    [Obsolete("THIS CLASS IS WIP")]
    internal class Assembly : Document, IAssembly
    {
        #region properties 

        public Components.IComponent RootComponent { get; set; }

        #endregion 

        #region events 

        public event EventHandler<ComponentAddedEventArgs> ComponentAdded = null;
        public event EventHandler<ComponentRemovedEventArgs> ComponentRemoved = null;

        // todo: not sure if we are going to need this in the future
        //public event EventHandler<ComponentStateChangedEventArgs> ComponentStateChanged = null;

        #endregion


        #region fields 


        #endregion 

        /// <summary>
        /// Initializes a new instance of the <see cref="Assembly"/> class.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="fullFileName">Full name of the file.</param>
        /// <param name="isRoot">if set to <c>true</c> [is root].</param>
        public Assembly(ModelDoc2 doc, string fullFileName, bool isRoot) : base(doc, fullFileName, isRoot)
        {

        }

        public void Initialize(string referencedConfiguration)
        {

            var configuration = (UnSafeObject as ModelDoc2).GetConfigurationByName(referencedConfiguration) as Configuration;

            var swRootComponent = configuration.GetRootComponent() as Component2;

            RootComponent = swRootComponent.ToIComponent();

            var components = new List<Components.IComponent>();

            var swComponents = assemblyDoc.GetComponents(true) as object[];



            if (swComponents != null)
            {
                foreach (var swComponent in swComponents)
                {
                    var component = swComponent.ToIComponent();

                    if (component != null)
                        components.Add(component);
                }
                
                RootComponent.Children = components.ToArray();                
            }


            // recursively init components

            Action<Components.IComponent> traverse = default(Action<Components.IComponent>);

            traverse = (Components.IComponent component) => {
                var assembly = (component.Document as IAssembly);
                if (assembly  != null)
                {
                    var cs = assembly.RootComponent.Children;
                    if (cs != null)
                    foreach (var c in cs)
                    {
                        c.Initialize();
                        traverse(c);
                    }
                }
                
            };

            traverse(this.RootComponent);
        }

        public override void AttachEventHandlers()
        {
            base.AttachEventHandlers();

            // attach event handlers 

            if (assemblyDoc != null)
            {
                assemblyDoc.AddItemNotify += AssemblyDoc_AddItemNotify;
                assemblyDoc.DeleteItemNotify += AssemblyDoc_DeleteItemNotify;
                assemblyDoc.ComponentStateChangeNotify3 += AssemblyDoc_ComponentStateChangeNotify3;
    
            }


        }

        private int AssemblyDoc_ComponentStateChangeNotify3(object Component, string CompName, short oldCompState, short newCompState)
        {
            var newState = (swComponentSuppressionState_e)newCompState;
            var component = Component as Component2;
            var documentManager = Globals.DocumentManager;
            var documents = documentManager.GetDocuments();
            var document = default(IDocument);

            switch (newState)
            {
                case swComponentSuppressionState_e.swComponentFullyResolved:
                       document = documents.FirstOrDefault(x => x.Equals(System.IO.Path.GetFileName(component.GetPathName())));
                    if (document == null)
                        documentManager.GetDocumentFromUnsafeObject(component.GetModelDoc2());
                    else
                    {
                        document.Load(component.GetModelDoc2());
                        document.Refresh();

                        if (document.DocumentType == swDocumentTypes_e.swDocASSEMBLY)
                        {
                            var assembly = document as IAssembly;
                            assembly.Initialize(component.ReferencedConfiguration);
                        }
                    }


                    break;
                case swComponentSuppressionState_e.swComponentResolved:
                    {
                        document = documents.FirstOrDefault(x => x.Equals(System.IO.Path.GetFileName(component.GetPathName())));
                        if (document == null)
                            documentManager.GetDocumentFromUnsafeObject(component.GetModelDoc2());
                    
                        else
                        {
                            document.Load(component.GetModelDoc2());
                            document.Refresh();
                        }
                    }
                    break;
                case swComponentSuppressionState_e.swComponentSuppressed:
                case swComponentSuppressionState_e.swComponentLightweight:
                case swComponentSuppressionState_e.swComponentFullyLightweight:
                case swComponentSuppressionState_e.swComponentInternalIdMismatch:
                default:
                    break;
            }



            return 0;
        }

        public override void DettachEventHandlers()
        {

            // attach event handlers 

            if (assemblyDoc != null)
            {
                assemblyDoc.AddItemNotify -= AssemblyDoc_AddItemNotify;
                assemblyDoc.DeleteItemNotify -= AssemblyDoc_DeleteItemNotify;
                assemblyDoc.ComponentStateChangeNotify3 -= AssemblyDoc_ComponentStateChangeNotify3;
             

            }

            base.AttachEventHandlers();

        }
     

        private int AssemblyDoc_AddItemNotify(int EntityType, string itemName)
        {
            var entityType = (swNotifyEntityType_e)EntityType;


            switch (entityType)
            {

                case swNotifyEntityType_e.swNotifyComponent:
                    {
                        var swComponent = this.assemblyDoc.GetComponentByName(itemName) as Component2;
                        var component = swComponent.ToIComponent();
                        RootComponent.AddChild(component);
                        ComponentAdded?.Invoke(this, ComponentAddedEventArgs.New(component));
                    }
                    break;
                case swNotifyEntityType_e.swNotifyConfiguration:
                    break;
                case swNotifyEntityType_e.swNotifyFeature:
                    break;
                case swNotifyEntityType_e.swNotifyDerivedConfiguration:
                    break;
                case swNotifyEntityType_e.swNotifyDrawingSheet:
                    break;
                case swNotifyEntityType_e.swNotifyDrawingView:
                    break;
                case swNotifyEntityType_e.swNotifyBlockDef:
                    break;
                case swNotifyEntityType_e.swNotifyComponentInternal:
                    break;
                default:
                    break;
            }

            return 0;
        }

        private int AssemblyDoc_DeleteItemNotify(int EntityType, string itemName)
        {
            var entityType = (swNotifyEntityType_e)EntityType;


            switch (entityType)
            {

                case swNotifyEntityType_e.swNotifyComponent:
                    {
                        var swComponent = this.assemblyDoc.GetComponentByName(itemName) as Component2;
                        var component = swComponent.ToIComponent();
                        RootComponent.RemoveChild(component);
                        ComponentRemoved?.Invoke(this, ComponentRemovedEventArgs.New(component));

                    }
                    break;
                case swNotifyEntityType_e.swNotifyConfiguration:
                    break;
                case swNotifyEntityType_e.swNotifyFeature:
                    break;
                case swNotifyEntityType_e.swNotifyDerivedConfiguration:
                    break;
                case swNotifyEntityType_e.swNotifyDrawingSheet:
                    break;
                case swNotifyEntityType_e.swNotifyDrawingView:
                    break;
                case swNotifyEntityType_e.swNotifyBlockDef:
                    break;
                case swNotifyEntityType_e.swNotifyComponentInternal:
                    break;
                default:
                    break;
            }

            return 0;
        }

    }
}



 