using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BlueByte.TestAddIn
{
    [ComVisible(true)]
    [Guid("BF1C1567-53D8-4E2B-B588-0518A1EBFA55")]
    [Name("Addin")]
    [Description("This is the description")]
    [StartUp(false)]
    [MenuItem("SDK", swDocumentTypes_e.swDocNONE, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocNONE, false, nameof(OnMenuClick))]
    public class AddIn : AddInBase
    { 

        public void OnMenuClick()
        {
            this.Application.SendMsgToUser("Hello World!");
        }

    }
}
