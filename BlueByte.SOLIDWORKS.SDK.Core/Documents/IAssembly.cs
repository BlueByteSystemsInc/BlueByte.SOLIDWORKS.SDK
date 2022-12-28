using BlueByte.SOLIDWORKS.SDK.Core.Documents.Components;
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
        /// <summary>
        /// Gets or sets the root component.
        /// </summary>
        /// <value>
        /// The root component.
        /// </value>
        IComponent RootComponent { get; set; }

        /// <summary>
        /// Initializes the assembly hierarchy.
        /// </summary>
        /// <param name="referencedConfiguration">The referenced configuration.</param>
        void Initialize(string referencedConfiguration);
    }
}