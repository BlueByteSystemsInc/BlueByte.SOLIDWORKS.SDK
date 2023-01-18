using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BlueByte.TestAddIn
{
    [ComVisible(true)]
    [Guid("BF1C1567-53D8-4E2B-B588-0518A1EBFA55")]

    [Name("Addin")]
    [Description("This is the description")]
    [StartUp(true)]
    
    [MenuItem("SDK", swDocumentTypes_e.swDocNONE, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocNONE, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
    [MenuItem("SDK", swDocumentTypes_e.swDocPART, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocPART, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
    [MenuItem("SDK", swDocumentTypes_e.swDocPART, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocASSEMBLY, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
    public class AddIn : AddInBase
    {
        #region fields 


        #endregion


        protected override void RegisterDefaultTypes()
        {
            base.RegisterDefaultTypes();

            

        }

        protected override void OnConnectToSOLIDWORKS(SldWorks swApp)
        {
            base.OnConnectToSOLIDWORKS(swApp);
            
            
            
            AttachDebugger();

       

        }

    

     
        protected override void OnDisconnectFromSOLIDWORKS()
        {
            base.OnDisconnectFromSOLIDWORKS();

        }


        #region menu event handlers 

        public void OnMenuClick()
        {

            var doc = this.DocumentManager.ActiveDocument;

            var extensions = FileExtension_e.x_t | FileExtension_e.stp| FileExtension_e.x_t | FileExtension_e.slddrw | FileExtension_e.sldprt | FileExtension_e.sldasm| FileExtension_e.Default |  FileExtension_e.igs | FileExtension_e.pdf;

            doc.Save(extensions);
        
        }

        private void DocumentManager_DocumentGotClosed(object sender, System.Tuple<IDocument, swDestroyNotifyType_e> e)
        {

            Debug.Print($"This document {e.Item1.FileName} was closed [{e.Item2.ToString()}].");
        }

        private void DocumentManager_DocumentGotCreated(object sender, IDocument e)
        {
            Debug.Print($"A new document has been created {e.FileName}.");

        }

        private void DocumentManager_DocumentGotOpened(object sender, IDocument e)
        {
            Debug.Print($"A document has been opened {e.FileName}.");

        }

        #endregion

    }
}
