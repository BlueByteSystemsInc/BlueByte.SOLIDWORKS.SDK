using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="BlueByte.SOLIDWORKS.SDK.Core.SOLIDWORKSObject" />
    /// <seealso cref="BlueByte.SOLIDWORKS.SDK.Core.ISOLIDWORKSApplication" />
    internal class SOLIDWORKSApplication : SOLIDWORKSObject, ISOLIDWORKSApplication
    {
        internal SOLIDWORKSApplication(SldWorks Sldworks) : base()
        {
            this.UnSafeObject = Sldworks;
        }


        public void SendWarningMessage(string message)
        {
            var swApp = this.UnSafeObject as SldWorks;

            swApp.SendMsgToUser2(message, (int)swMessageBoxIcon_e.swMbWarning, (int)swMessageBoxBtn_e.swMbOk);


        }

        public void SendErrorMessage(string message)
        {
            var swApp = this.UnSafeObject as SldWorks;
            swApp.SendMsgToUser2(message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
        }


        public void SendInformationMessage(string message)
        {
            var swApp = this.UnSafeObject as SldWorks;
            swApp.SendMsgToUser2(message, (int)swMessageBoxIcon_e.swMbStop, (int)swMessageBoxBtn_e.swMbOk);
        }
    }


}
