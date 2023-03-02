using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

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

        public void Dispose()
        {
            this.UnSafeObject = null;
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
        public string GetLastSaveErrorMessage()
        {
            var swApp = this.UnSafeObject as SldWorks;
            object varError;
            object varPath;
            var varMessage = swApp.GetLastSaveError(out varPath, out varError);

            if (varError == null)
                return string.Empty;

            return varMessage.ToString();
        }

        public string[] GetRecentErrors()
        {
            var swApp = this.UnSafeObject as SldWorks;

            object msgs;
            object msgids;
            object msgtypes;
             
            swApp.GetErrorMessages(out msgs, out msgids, out msgtypes);

            var msgsArr = msgs as string[];

            return msgsArr;
        }


        
    }


}
