using BlueByte.SOLIDWORKS.SDK.Core.Documents.Components;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    [Obsolete("THIS CLASS IS WIP")]
    internal class Assembly : Document, IAssembly
    {
        #region properties 

        public Components.IComponent RootComponent { get; set; }

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
            }


        }


        public override void DettachEventHandlers()
        {

            // attach event handlers 

            if (assemblyDoc != null)
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
}



 