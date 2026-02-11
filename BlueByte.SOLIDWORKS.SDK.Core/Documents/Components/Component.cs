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


        public swDocumentTypes_e GetComponentType()
        {
            var name = this.UnsafeObject as SolidWorks.Interop.sldworks.Component2;

            if (name == null)
                throw new Exception("Unsafeobject object did not cast API object");

            var extension = name.GetPathName().ToLower();


            return extension.EndsWith(".sldprt") ? swDocumentTypes_e.swDocPART : swDocumentTypes_e.swDocASSEMBLY;


        }

        public bool IsPatternInstance { get; set; }

        public bool IsSmartComponent { get; set; }

        public bool IsVirtual { get; set; }

        public swComponentSuppressionState_e SuppressionState { get; set; }
        
        public bool IsSpeedPak { get; set; }

        public IComponent[] Children { get; set; } = new IComponent[] { };

        public dynamic UnsafeObject { get; set; }

        public string ComponentReference { get; set; }

        public IComponent   Parent { get; set; }


        public string ReferencedConfiguration { get; set; }

        public IDocument Document { get; set; }

        public bool ExcludedFromBOM { get;  set; }

        public string PathName { get; set; }


        public bool IsEnvelope { get;   set; }

        public bool Initialize()
        {
            if (this.Document == null)
                return false;

            if (this.Document.DocumentType != swDocumentTypes_e.swDocASSEMBLY)
                return false;


            if (this.SuppressionState  == swComponentSuppressionState_e.swComponentSuppressed)
                return false;


            var childrenComponents = (UnsafeObject as SolidWorks.Interop.sldworks.Component2).GetChildren() as Object[];


            foreach (var child in childrenComponents)
            {
                var swChild = child as SolidWorks.Interop.sldworks.Component2;

                if (swChild == null)
                    continue;

                var component = swChild.ToIComponent();



                component.Parent = this;

                if (Children == null)
                    Children = new IComponent[] { component };
                else
                {
                    var t = new System.Collections.Generic.List<IComponent>();
                    t.AddRange(this.Children);
                    t.Add(component);
                    this.Children = t.ToArray();
                }
              
            }

           
            if (this.Children != null)
             foreach (var child in this.Children)
             {
                   var childRet = child.Initialize();
             }


            return true; 

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

 