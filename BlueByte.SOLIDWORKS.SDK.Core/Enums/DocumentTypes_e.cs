using System.ComponentModel;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Enums
{
    [Flags]
    public enum DocumentTypes_e
    {
        [Description("No document")]
        swDocNONE = 0,

        [Description("SOLIDWORKS Part document (*.sldprt)")]
        swDocPART = 1 << 0, // 1

        [Description("SOLIDWORKS Assembly document (*.sldasm)")]
        swDocASSEMBLY = 1 << 1, // 2

        [Description("SOLIDWORKS Drawing document (*.slddrw)")]
        swDocDRAWING = 1 << 2, // 4

        [Description("SOLIDWORKS Design Modeler document")]
        swDocSDM = 1 << 3, // 8

        [Description("SOLIDWORKS Layout document")]
        swDocLAYOUT = 1 << 4, // 16

        [Description("Imported Part document (e.g., STEP, IGES opened as part)")]
        swDocIMPORTED_PART = 1 << 5, // 32

        [Description("Imported Assembly document (e.g., STEP, IGES opened as assembly)")]
        swDocIMPORTED_ASSEMBLY = 1 << 6 // 64
    }
}


 