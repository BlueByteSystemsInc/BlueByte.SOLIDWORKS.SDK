using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Core;
using System;
using System.Runtime.InteropServices;

namespace Template
{
    [ComVisible(true)]
    [Guid("BF1C1567-53D8-4E2B-B588-0518A1EBFA55")]

    [Name("Addin")]
    [Description("This is the description")]
    [StartUp(true)]
    public class AddIn : AddInBase
    {

    }
}
