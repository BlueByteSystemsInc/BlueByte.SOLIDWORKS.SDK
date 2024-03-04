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


        public static string GetPathName2(this Component2 component)
        {
            var pathName = component.GetPathName();

            var m = component.GetModelDoc2() as ModelDoc2;
            if (m != null)
                pathName = m.GetPathName();

            var illegals = new char[] { };

            illegals = System.IO.Path.GetInvalidPathChars();




            if (pathName.IndexOfAny(illegals) >= 0)
            {
                pathName = component.GetPathName().Split(new char[] { '\\' }).LastOrDefault();
                pathName = System.IO.Path.Combine(@"C:\Temp\VirtualComponents\", pathName);
            }



            return pathName;

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

            swComponent.ComponentReference = component.ComponentReference;

            swComponent.IsSpeedPak = component.IsSpeedPak;

            swComponent.ExcludedFromBOM = component.ExcludeFromBOM;

            swComponent.IsEnvelope = component.IsEnvelope();

            swComponent.IsSmartComponent = component.IsSmartComponent();


            swComponent.PathName = component.GetPathName2();

            var documentManager = Globals.DocumentManager;

            var document = default(IDocument);

            var documents = documentManager.GetDocuments().ToList();

            var pathName = component.GetPathName();

            if (pathName.Contains("<"))
                pathName = component.GetPathName2();


            document = documents.FirstOrDefault(x => x.Equals(System.IO.Path.GetFileName(pathName)));



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
                    documentManager.AddUnloadedDocument(pathName);
                   break;
               
            }


            documents = documentManager.GetDocuments().ToList();
            document = documents.FirstOrDefault(x => x.Equals(System.IO.Path.GetFileName(pathName)));


            swComponent.Document = document;

           

            // this throws an memory access violation
            //swComponent.IsPatternInstance = component.IsPatternInstance();
            



            return swComponent;
        }


    
    }
}

 