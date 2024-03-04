using BlueByte.SOLIDWORKS.SDK.Core.BillOfMaterials;
using BlueByte.SOLIDWORKS.SDK.Core.Documents.Components;
using BlueByte.SOLIDWORKS.SDK.Core.Models;
using System;
using System.Collections.Generic;

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


        /// <summary>
        /// Traverses the assembly and do.
        /// </summary>
        /// <param name="doAction">The do action.</param>
        void TraverseAndDo(Action<Components.IComponent> doAction);
        /// <summary>
        /// Gets the quantitied references.
        /// </summary>
        /// <param name="bomSettings">The bom settings.</param>
        /// <returns></returns>
        List<Stuple<string, int>> GetQuantitiedReferences(BOMSettings bomSettings);
    }
}