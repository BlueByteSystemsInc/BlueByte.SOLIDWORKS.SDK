using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
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
    public class AddIn : AddInBase
    {
        #region fields 
        IDocumentManager documentManager;

        #endregion 


        protected override void OnDisconnectFromSOLIDWORKS()
        {
            base.OnDisconnectFromSOLIDWORKS();
            documentManager.DeattachEventHandlers();
        }


        #region menu event handlers 

        public void OnMenuClick()
        {
            AttachDebugger();

            var app = Container.GetInstance<SOLIDWORKSApplication>();
            
            documentManager = Container.GetInstance<IDocumentManager>();
            documentManager.LoadExistingDocuments();

            app.As<SldWorks>().SendMsgToUser($"Hello World! There are {documentManager.Documents.Count} open.");


        }

        #endregion 

    }
}
