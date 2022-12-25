using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    internal class SOLIDWORKSApplication : SOLIDWORKSObject, ISOLIDWORKSApplication
    {
        internal SOLIDWORKSApplication(SldWorks Sldworks) : base()
        {
            this.UnSafeObject = Sldworks;
        }
    }


}
