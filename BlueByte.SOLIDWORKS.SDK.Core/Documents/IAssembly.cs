using BlueByte.SOLIDWORKS.SDK.Core.BillOfMaterials;
using BlueByte.SOLIDWORKS.SDK.Core.Documents.Components;
using BlueByte.SOLIDWORKS.SDK.Core.Models;
using System;
using System.Collections.Generic;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{

   
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
        bool Initialize(string referencedConfiguration);


        /// <summary>
        /// Traverses the assembly and do.
        /// </summary>
        /// <param name="doAction">The do action.</param>
        void TraverseAndDo(Action<Components.IComponent> doAction);


        /// <summary>
        /// Traverses the assembly and continues until the action returns false.
        /// </summary>
        /// <param name="doAction">The do action.</param>
        void TraverseAndContinue(Func<Components.IComponent, bool> continueAction);



        /// <summary>
        /// Gets the quantitied references.
        /// </summary>
        /// <param name="bomSettings">The bom settings.</param>
        /// <returns></returns>
        List<Stuple<string, int>> GetFlatBOM(BOMSettings bomSettings);


        /// <summary>
        /// Returns the flat BOM and allows grouping by a property name.
        /// </summary>
        /// <param name="bomSettings"></param>
        /// <param name="groupby"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        List<Stuple<string, double>> GetFlatBOM(BOMSettings bomSettings, GroupBy groupby = GroupBy.FileName, string propertyName = "");


    }
}