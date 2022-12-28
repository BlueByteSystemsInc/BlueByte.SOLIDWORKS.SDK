using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{
    public class Component : IComponent
    {
        public static IDocumentManager DocumentManager { get; set; }

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
    }
}

 