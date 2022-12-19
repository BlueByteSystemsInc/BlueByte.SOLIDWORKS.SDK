using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSDK.TestAddIn
{


    [MenuItem(nameof(OnMenuClick), swDocumentTypes_e.swDocPART, "","","","")]
    public class AddIn : AddInBase
    {

        protected override void ConnectToSOLIDWORKS(SldWorks swApp)
        {
            base.ConnectToSOLIDWORKS(swApp);
        }

        public void OnMenuClick()
        {

        }


    }
}
