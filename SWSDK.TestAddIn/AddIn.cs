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

        public void OnMenuClick()
        {
            var app = Container.GetInstance<SldWorks>();
            app.SendMsgToUser("Hello World!");


            var documentManager = Container.GetInstance<IDocumentManager>();



        }


        
    }
}
