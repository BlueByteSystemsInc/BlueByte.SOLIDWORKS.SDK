using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{

    [ObsoleteAttribute("THIS TYPE IS WIP")]
    /// <summary>
    /// Represents a wrapper over <see cref="SolidWorks.Interop.sldworks.AssemblyDoc"/>. Contains methods and events specific to component management.
    /// </summary>
    /// <seealso cref="BlueByte.SOLIDWORKS.SDK.Core.Documents.IDocument" />
    public interface IAssembly : IDocument
    {
        IComponent RootComponent { get; set; }
    }
}