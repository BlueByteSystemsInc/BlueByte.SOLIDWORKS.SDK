using System;
using System.ComponentModel;

namespace BlueByte.SOLIDWORKS.SDK.Core.Enums
{
    [Flags]
    public enum swDescriptiveFileWarningError_e
    {
        [Description("There was a rebuild error.")]
        swFileSaveWarning_RebuildError = 1,
        [Description("This file needs a rebuild.")]
        swFileSaveWarning_NeedsRebuild = 2,
        [Description("The views in this file needs an update.")]
        swFileSaveWarning_ViewsNeedUpdate = 4,
        [Description("The animator needs to solve.")]
        swFileSaveWarning_AnimatorNeedToSolve = 8,
        [Description("The animator feature edits.")]
        swFileSaveWarning_AnimatorFeatureEdits = 16,
        [Description("Edrawings bad selection.")]
        swFileSaveWarning_EdrwingsBadSelection = 32,
        [Description("The animator light edits.")]
        swFileSaveWarning_AnimatorLightEdits = 64,
        [Description("Animator camera views.")]
        swFileSaveWarning_AnimatorCameraViews = 128,
        [Description("Animator section views.")]
        swFileSaveWarning_AnimatorSectionViews = 256,
        [Description("There is a missing OLE object.")]
        swFileSaveWarning_MissingOLEObjects = 512,
        [Description("The file was opened in viewonly mode.")]
        swFileSaveWarning_OpenedViewOnly = 1024,
        [Description("Invalid XML.")]
        swFileSaveWarning_XmlInvalid = 2048
    }
}
