using BlueByte.SOLIDWORKS.SDK.Core.BillOfMaterials;
using BlueByte.SOLIDWORKS.SDK.Core.Documents.Components;
using BlueByte.SOLIDWORKS.SDK.Core.Models;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{

    public enum GroupBy
    {
        None,
        FileName,
        PartNumber,
        ConfigurationName,
        CustomProperty
    }

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

        /// <summary>
        /// Initializes the assembly hierarchy.
        /// </summary>
        /// <param name="referencedConfiguration">The referenced configuration.</param>
        public bool Initialize(string referencedConfiguration)
        {
 
            var configuration = (UnSafeObject as ModelDoc2).GetConfigurationByName(referencedConfiguration) as Configuration;

            var swRootComponent = configuration.GetRootComponent() as Component2;

            RootComponent = swRootComponent.ToIComponent();

            var components = new List<Components.IComponent>();

            var swComponents = (UnSafeObject as AssemblyDoc).GetComponents(true) as object[];

            if (swComponents != null)
            {
                foreach (var swComponent in swComponents)
                {
                    var component = swComponent.ToIComponent();

                    component.Parent = RootComponent;

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
                        c.Parent = assembly.RootComponent;
                        var ret = c.Initialize();
                        traverse(c);
                    }
                }
                
            };

            traverse(this.RootComponent);

            return true;
        }


        /// <summary>
        /// Traverses the assembly component tree and do.
        /// </summary>
        /// <param name="doAction">The do action.</param>
        public void TraverseAndDo(Action<Components.IComponent> doAction)
        {
            Action<Components.IComponent> traverse = default(Action<Components.IComponent>);

            traverse = (Components.IComponent component) => {
 
                if (doAction != null)
                    doAction.Invoke(component);

                var children = component.Children;

                foreach (var child in children)
                    traverse(child);
            };


            traverse(RootComponent);
        }

        internal class Row
        {
            public string FileName { get; set; }

            public Row Parent { get; set; }

            public List<Row> Children { get; set; } = new List<Row>();

            public int RowQuantity { get; set; }

            public static Row FromComponent(Components.IComponent component)
            {
                var row = new Row();
                row.FileName = component.Document.FileName;
                row.RowQuantity = 1;
                return row; 

            }
        }


    

        public List<Stuple<string, int>> GetFlatBOM(BOMSettings bomSettings)
        {
         

            var l = new List<Stuple<string, int>>();

            var rootComponent = this.RootComponent;

            Action<Components.IComponent> traverse = default(Action<Components.IComponent>);

            traverse = (Components.IComponent x) => 
            {

                if (bomSettings.IgnoreBOMExcludedComponents && x.ExcludedFromBOM)
                    return;

                if (bomSettings.IgnoreVirtualComponents && x.IsVirtual)
                    return;

                if (bomSettings.IgnoreEnvelopeComponents && x.IsEnvelope)
                    return;
                
                if (x.SuppressionState == swComponentSuppressionState_e.swComponentSuppressed)
                    return;

                var f = System.IO.Path.GetFileName(x.PathName);

                 

                var i = l.FirstOrDefault(j => j.Item1.Equals(f, StringComparison.OrdinalIgnoreCase));

                if (i == null)
                {
                    if (string.IsNullOrWhiteSpace(f) == false)
                        l.Add(new Stuple<string, int>(f, 1));
                }
                else
                    i.Item2 = i.Item2 + 1;


                if (x.Children != null)
                foreach (var component in x.Children)
                    traverse(component);

            };


            traverse(rootComponent);

            return l;
        }


        public List<Stuple<string, double>> GetFlatBOM(BOMSettings bomSettings, GroupBy groupby = GroupBy.FileName, string propertyName = "")
        {


            var l = new List<Stuple<string, double>>();

            var rootComponent = this.RootComponent;

            Action<Components.IComponent> traverse = default(Action<Components.IComponent>);

            traverse = (Components.IComponent x) =>
            {

                if (bomSettings.IgnoreBOMExcludedComponents && x.ExcludedFromBOM)
                    return;

                if (bomSettings.IgnoreVirtualComponents && x.IsVirtual)
                    return;

                if (bomSettings.IgnoreEnvelopeComponents && x.IsEnvelope)
                    return;

                if (x.SuppressionState == swComponentSuppressionState_e.swComponentSuppressed)
                    return;

                var identifierX = System.IO.Path.GetFileName(x.PathName);
                switch (groupby)
                {
                    case GroupBy.None:
                        break;
                    case GroupBy.FileName:
                        identifierX = System.IO.Path.GetFileName(x.PathName);
                        break;
                    case GroupBy.PartNumber:
                        {
                           identifierX = string.Empty;
                            if (x.Document.CustomPropertyManager.TryGet(x.Document, "PartNumber", x.ReferencedConfiguration, out string partNumber))
                                identifierX = partNumber;
                            if (string.IsNullOrWhiteSpace(identifierX))
                                identifierX = string.Empty;
                        }
                        break;
                    case GroupBy.ConfigurationName:
                        identifierX = x.ReferencedConfiguration;
                        break;
                    case GroupBy.CustomProperty:
                        identifierX = string.Empty;
                        if (x.Document.CustomPropertyManager.TryGet(x.Document, propertyName, x.ReferencedConfiguration, out string propertyValue))
                            identifierX = propertyValue;
                        if (string.IsNullOrWhiteSpace(identifierX))
                            identifierX = string.Empty;
                        break;
                    default:
                        break;
                }
                



                var i = l.FirstOrDefault(j => j.Item1.Equals(identifierX, StringComparison.OrdinalIgnoreCase));

                if (i == null)
                {
                    if (string.IsNullOrWhiteSpace(identifierX) == false)
                    {
                        var quantity = 1.0;

                        var ret = x.Document.CustomPropertyManager.TryGet(x.Document, "UNIT_OF_MEASURE", x.ReferencedConfiguration, out string bomQuantityPropertyName);

                        if (ret)
                            l.Add(new Stuple<string, double>(identifierX, quantity));
                        else
                        {
                            ret = x.Document.CustomPropertyManager.TryGet(x.Document, bomQuantityPropertyName, x.ReferencedConfiguration, out string quantityStr);

                            ret = double.TryParse(quantityStr, out quantity);
                        }

                        l.Add(new Stuple<string, double>(identifierX, quantity));

                    }

                }
                else
                {
                    double quantity = 1;
                    var ret = x.Document.CustomPropertyManager.TryGet(x.Document, "UNIT_OF_MEASURE", x.ReferencedConfiguration, out string bomQuantityPropertyName);

                    if (ret)
                        l.Add(new Stuple<string, double>(identifierX, quantity));
                    else
                    {
                        ret = x.Document.CustomPropertyManager.TryGet(x.Document, bomQuantityPropertyName, x.ReferencedConfiguration, out string quantityStr);

                        ret = double.TryParse(quantityStr, out quantity);
                    }

                    l.Add(new Stuple<string, double>(identifierX, quantity));

                    i.Item2 = i.Item2 + quantity;
                }

                if (x.Children != null)
                    foreach (var component in x.Children)
                        traverse(component);

            };


            traverse(rootComponent);

            return l;
        }


        private int AssemblyDoc_ComponentConfigurationChangeNotify(string componentName, string oldConfigurationName, string newConfigurationName)
        {
            var swComponent = this.assemblyDoc.GetComponentByName(componentName) as Component2;

            var app = Globals.Application.As<SldWorks>();

            var component = this.RootComponent.Children.FirstOrDefault(x => app.IsSame(swComponent,x.UnsafeObject as Component2) == (int)swObjectEquality.swObjectSame);

            if (component != null)
                component.ReferencedConfiguration = newConfigurationName; 

            return 0;
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
        public override void AttachEventHandlers()
        {
            base.AttachEventHandlers();

            // attach event handlers 

            if (assemblyDoc != null)
            {
                assemblyDoc.AddItemNotify += AssemblyDoc_AddItemNotify;
                assemblyDoc.DeleteItemNotify += AssemblyDoc_DeleteItemNotify;
                assemblyDoc.ComponentStateChangeNotify3 += AssemblyDoc_ComponentStateChangeNotify3;
                assemblyDoc.ComponentConfigurationChangeNotify += AssemblyDoc_ComponentConfigurationChangeNotify;
          
            }


        }
 

        public override void DettachEventHandlers()
        {

            // dettach event handlers 

            if (assemblyDoc != null)
            {
                assemblyDoc.AddItemNotify -= AssemblyDoc_AddItemNotify;
                assemblyDoc.DeleteItemNotify -= AssemblyDoc_DeleteItemNotify;
                assemblyDoc.ComponentStateChangeNotify3 -= AssemblyDoc_ComponentStateChangeNotify3;
                assemblyDoc.ComponentConfigurationChangeNotify -= AssemblyDoc_ComponentConfigurationChangeNotify;
              

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
                        component.Parent = RootComponent;
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
                case swNotifyEntityType_e.swNotifyFeature:
                case swNotifyEntityType_e.swNotifyDerivedConfiguration:
                case swNotifyEntityType_e.swNotifyDrawingSheet:
                case swNotifyEntityType_e.swNotifyDrawingView:
                case swNotifyEntityType_e.swNotifyBlockDef:
                case swNotifyEntityType_e.swNotifyComponentInternal:
                    break;
                default:
                    break;
            }

            return 0;
        }

    }
}



 