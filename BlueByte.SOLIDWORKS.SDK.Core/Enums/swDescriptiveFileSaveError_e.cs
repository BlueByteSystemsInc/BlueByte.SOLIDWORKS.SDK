using System;
using System.ComponentModel;

namespace BlueByte.SOLIDWORKS.SDK.Core.Enums
{
    [Flags]
    public enum swDescriptiveFileSaveError_e
    {
        [Description("No errors.")]
        Default = 0,
        [Description("Generic error.")]
        swGenericSaveError = 1,
        [Description("File is read-only.")]
        swReadOnlySaveError = 2,
        [Description("File name cannot be empty.")]
        swFileNameEmpty = 4,
        [Description("File name cannot contain the at symbol (@).")]
        swFileNameContainsAtSign = 8,
        [Description("This file is locked.")]
        swFileLockError = 16,
        [Description("Save As file type is not valid.")]
        swFileSaveFormatNotAvailable = 32,
        [Description("File saved but with rebuild errors. Errors: https://help.solidworks.com/2017/english/api/swconst/SolidWorks.Interop.swconst~SolidWorks.Interop.swconst.swFileSaveWarning_e.html")]
        swFileSaveWithRebuildError = 64,
        [Description("Do not overwrite an existing file.")]
        swFileSaveAsDoNotOverwrite = 128,
        [Description("File name extension does not match the SOLIDWORKS document type.")]
        swFileSaveAsInvalidFileExtension = 256,
        [Description("Save the selected bodies in a part document. Valid option for IPartDoc::SaveToFile2; however, not a valid option for IModelDocExtension::SaveAs")]
        swFileSaveAsNoSelection = 512,
        [Description("Bad eDrawings Version.")]
        swFileSaveAsBadEDrawingsVersion = 1024,
        [Description("File name cannot exceed 255 characters.")]
        swFileSaveAsNameExceedsMaxPathLength = 2048,
        [Description("Save As operation is not supported or was executed is such a way that the resulting file might not be complete, possibly because SOLIDWORKS is hidden; if the error persists after setting SOLIDWORKS to visible and re-attempting the Save As operation, contact SOLIDWORKS API support.")]
        swFileSaveAsNotSupported = 4096,
        [Description("Saving an assembly with renamed components requires saving the references.")]
        swFileSaveRequiresSavingReferences = 8192
    }
}
