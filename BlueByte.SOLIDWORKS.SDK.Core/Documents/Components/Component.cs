using SolidWorks.Interop.swconst;
using System;
using System.Linq;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{


    public class Component : IComponent
    {

        public string GetNameRelativeTo(IComponent rootComponent)
        {
            throw new NotImplementedException();
        }

        public string GetSelectionRelativeTo(IComponent rootComponent)
        {
            throw new NotImplementedException();
        }

        public bool IsPatternInstance { get; set; }

        public bool IsSmartComponent { get; set; }

        public bool IsVirtual { get; set; }

        public swComponentSuppressionState_e SuppressionState { get; set; }
        
        public bool IsSpeedPak { get; set; }

        public IComponent[] Children { get; set; }

        public dynamic UnsafeObject { get; set; }

        public string ReferencedConfiguration { get; set; }

        public IDocument Document { get; set; }


        public void Initialize()
        {
            if (this.Document == null)
                return;

            if (this.Document.DocumentType != swDocumentTypes_e.swDocASSEMBLY)
                return;


            if (this.SuppressionState != swComponentSuppressionState_e.swComponentFullyResolved && this.SuppressionState != swComponentSuppressionState_e.swComponentResolved)
                return;

            var assembly = this.Document as IAssembly;

            
            if (assembly.RootComponent == null)
                assembly.Initialize(ReferencedConfiguration);


            this.Children = assembly.RootComponent.Children;

            foreach (var child in this.Children)
                child.Initialize();


        }



        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <exception cref="System.ArgumentNullException">child</exception>
        public void AddChild(IComponent child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (this.Children == null)
                this.Children = new IComponent[] { };


            

            var l = this.Children.ToList();

            l.Add(child);

            this.Children = l.ToArray();
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <exception cref="System.ArgumentNullException">child</exception>
        public void RemoveChild(IComponent child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (this.Children == null)
                return;




            var l = this.Children.ToList();

            l.Remove(child);

            this.Children = l.ToArray();
        }


        public override string ToString()
        {
            if (this.Document != null)
                return $"{this.Document.FileName} [{ReferencedConfiguration}]";

            return base.ToString();
        }
    }
}

 