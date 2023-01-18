using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Enums
{
    /// <summary>
    /// File extension. Supports bitmask. Use <see cref="Enum.HasFlag(Enum)"/> to check if the member for the desired extension is included/
    /// </summary>
    [Flags]
    public enum FileExtension_e
    {
        /// <summary>
        /// Save documents to one of the three SOLIDWORKS document types depending on the model type.
        /// </summary>
        Default,
        /// <summary>
        /// SOLIDWORKS part document.
        /// </summary>
        sldprt,
        /// <summary>
        /// SOLIDWORKS assembly document.
        /// </summary>
        sldasm,
        /// <summary>
        /// SOLIDWORKS drawing document.
        /// </summary>
        slddrw, 
        /// <summary>
        /// STP file.
        /// </summary>
        stp, 
        /// <summary>
        /// Parasolid file.
        /// </summary>
        x_t,
        /// <summary>
        /// PDF.
        /// </summary>
        pdf, 
        /// <summary>
        /// IGS
        /// </summary>
        igs

    }
}
