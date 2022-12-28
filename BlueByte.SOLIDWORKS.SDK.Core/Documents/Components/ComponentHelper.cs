using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Linq;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{
    public static class ComponentHelper
    {
        public static IComponent ToIComponent(this object component)
        {

            var swComponent = component as Component2;

            return swComponent.ToIComponent();

        }
        public static IComponent ToIComponent(this Component2 component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));


            var swComponent = new Component();

            swComponent.SuppressionState = (swComponentSuppressionState_e)component.GetSuppression2();

            swComponent.ReferencedConfiguration = component.ReferencedConfiguration;

            swComponent.UnsafeObject = component;

            swComponent.IsVirtual = component.IsVirtual;

            swComponent.IsSpeedPak = component.IsSpeedPak;

            swComponent.IsSmartComponent = component.IsSmartComponent();

            var documentManager = Component.DocumentManager;

            var document = default(IDocument);

            var documents = documentManager.GetDocuments().ToList();
            
            document = documents.FirstOrDefault(x => x.Equals(System.IO.Path.GetFileName(component.GetPathName())));



            switch (swComponent.SuppressionState)
            {
                case swComponentSuppressionState_e.swComponentFullyResolved:
                case swComponentSuppressionState_e.swComponentResolved:
                    if (document == null)
                        documentManager.GetDocumentFromUnsafeObject(component.GetModelDoc2());
                   break;
                case swComponentSuppressionState_e.swComponentSuppressed:
                case swComponentSuppressionState_e.swComponentLightweight:
                case swComponentSuppressionState_e.swComponentFullyLightweight:
                case swComponentSuppressionState_e.swComponentInternalIdMismatch:
                default:
                    documentManager.AddUnloadedDocument(component.GetPathName());
                   break;
               
            }


            documents = documentManager.GetDocuments().ToList();
            document = documents.FirstOrDefault(x => x.Equals(System.IO.Path.GetFileName(component.GetPathName())));


            swComponent.Document = document;

           

            // this throws an memory access violation
            //swComponent.IsPatternInstance = component.IsPatternInstance();
            



            return swComponent;
        }


    
    }
}

 