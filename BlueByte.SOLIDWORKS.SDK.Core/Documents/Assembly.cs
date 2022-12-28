using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    [ObsoleteAttribute("THIS CLASS IS WIP")]
    internal class Assembly : Document, IAssembly
    {
        #region properties 

        public IComponent RootComponent { get; set; }

        #endregion 

        #region events 

        public event EventHandler<ComponentAddedEventArgs> ComponentAdded;
        public event EventHandler<ComponentRemovedEventArgs> ComponentRemoved;
        public event EventHandler<ComponentStateChangedEventArgs> ComponentStateChanged;

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

        public void Initialize()
        {

        }

        public override void AttachEventHandlers()
        {
            base.AttachEventHandlers();

            // attach event handlers 

            if (this.assemblyDoc != null)
            {
                assemblyDoc.AddItemNotify += AssemblyDoc_AddItemNotify;
                assemblyDoc.DeleteItemNotify += AssemblyDoc_DeleteItemNotify;
            }

            
        }

       
        public override void DettachEventHandlers()
        {
            
            // attach event handlers 

            if (this.assemblyDoc != null)
            {
                assemblyDoc.AddItemNotify -= AssemblyDoc_AddItemNotify;
                assemblyDoc.DeleteItemNotify -= AssemblyDoc_DeleteItemNotify;

            }

            base.AttachEventHandlers();

        }

        private int AssemblyDoc_AddItemNotify(int EntityType, string itemName)
        {



            return 0;
        }

        private int AssemblyDoc_DeleteItemNotify(int EntityType, string itemName)
        {
            return 0;
        }

    }

    public interface IComponent
    {
        IComponent[] Children { get; set; }
        
        IDocument Document { get; set; }
        
        string ReferencedConfiguration { get; set; }
        
        string SelectionString { get; set; }

        SOLIDWORKSObject SOLIDWORKSObject { get; set; }

        string GetNameRelativeTo(IComponent rootComponent);
        
        string GetSelectionRelativeTo(IComponent rootComponent);
        
        void Initialize();
    }

    public class Component : IComponent
    {
        public string SelectionString { get; set; }


        public string GetNameRelativeTo(IComponent rootComponent)
        {
            throw new NotImplementedException();
        }
        public string GetSelectionRelativeTo(IComponent rootComponent)
        {
            throw new NotImplementedException();
        }

        public IComponent[] Children { get; set; }

        public SOLIDWORKSObject SOLIDWORKSObject { get; set; }

        public string ReferencedConfiguration { get; set; }

        public IDocument Document { get; set; }


        public void Initialize()
        {


            #region initialize by getting immediate children

            var assembly = this.Document as IAssembly; 
            if (assembly != null)
            {
                // this document has been initialized 
                if (assembly.RootComponent == null)
                    return; 

                var configuration = this.Document.As<ModelDoc2>().GetConfigurationByName(ReferencedConfiguration) as Configuration;

                var swRootComponent = configuration.GetRootComponent() as Component2;

                assembly.RootComponent = swRootComponent.ToIComponent();

                var components = new List<IComponent>();

                var swComponents = assembly.As<AssemblyDoc>().GetComponents(true) as object[];

                if (swComponents != null)
                {
                    foreach (var swComponent in swComponents)
                    {
                        var component = swComponent.ToIComponent();

                        if (component != null)
                            components.Add(component);
                    }

                    this.Children = components.ToArray();
                }
            }

            #endregion 


            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child.Initialize();
                }
            }
        }
    }

    public static class ComponentHelper
    {
        public static IComponent ToIComponent(this object component)
        {

            var swComponent = component as Component2;

            return ToIComponent(swComponent);

        }
        public static IComponent ToIComponent(this Component2 component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            throw new NotImplementedException();
        }
    }


    public class ComponentAddedEventArgs : EventArgs
    {

    }
    public class ComponentRemovedEventArgs : EventArgs
    {

    }
    public class ComponentStateChangedEventArgs : EventArgs
    {

    }
}