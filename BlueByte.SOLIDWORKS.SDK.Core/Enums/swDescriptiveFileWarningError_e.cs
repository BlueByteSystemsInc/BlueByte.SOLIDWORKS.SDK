using System;
using System.ComponentModel;

namespace BlueByte.SOLIDWORKS.SDK.Core.Enums
{
    [Flags]
    public enum swDescriptiveFileWarningError_e
    {
        [Description()]
        swFileSaveWarning_RebuildError = 1,
        [Description()]
        swFileSaveWarning_NeedsRebuild = 2,
        [Description()]
        swFileSaveWarning_ViewsNeedUpdate = 4,
        [Description()]
        swFileSaveWarning_AnimatorNeedToSolve = 8,
        [Description()]
        swFileSaveWarning_AnimatorFeatureEdits = 16,
        [Description()]
        swFileSaveWarning_EdrwingsBadSelection = 32,
        [Description()]
        swFileSaveWarning_AnimatorLightEdits = 64,
        [Description()]
        swFileSaveWarning_AnimatorCameraViews = 128,
        [Description()]
        swFileSaveWarning_AnimatorSectionViews = 256,
        [Description()]
        swFileSaveWarning_MissingOLEObjects = 512,
        [Description()]
        swFileSaveWarning_OpenedViewOnly = 1024,
        [Description()]
        swFileSaveWarning_XmlInvalid = 2048
    }
}
