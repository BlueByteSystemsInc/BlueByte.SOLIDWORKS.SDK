using SolidWorks.Interop.sldworks;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    public class SOLIDWORKSApplication : SOLIDWORKSObject
    {
        public SOLIDWORKSApplication(SldWorks Sldworks) : base()
        {
            this.UnSafeObject = Sldworks;
        }
    }
}