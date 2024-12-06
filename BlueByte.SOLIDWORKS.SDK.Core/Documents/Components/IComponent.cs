using SolidWorks.Interop.swconst;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{
    /// <summary>
    /// Component interface.
    /// </summary>
    public interface IComponent
    {

        /// <summary>
        /// Gets the type of the component.
        /// </summary>
        /// <returns></returns>
        swDocumentTypes_e GetComponentType();


        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        IComponent Parent { get; set; }


        /// <summary>
        /// Gets or sets the component reference.
        /// </summary>
        /// <value>
        /// The component reference.
        /// </value>
        string ComponentReference { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        IComponent[] Children { get; set; }


        /// <summary>
        /// Gets or sets the name of the path.
        /// </summary>
        /// </summary>
        /// <value>
        /// The name of the path.
        /// </value>
       string PathName { get; set; }


        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="child">The child.</param>
        void AddChild(IComponent child);

        /// <summary>
        /// Removes the child.
        /// </summary>
        /// <param name="child">The child.</param>
        void RemoveChild(IComponent child);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is virtual; otherwise, <c>false</c>.
        /// </value>
        bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the state of the suppression.
        /// </summary>
        /// <value>
        /// The state of the suppression.
        /// </value>
        swComponentSuppressionState_e SuppressionState { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        IDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the referenced configuration.
        /// </summary>
        /// <value>
        /// The referenced configuration.
        /// </value>
        string ReferencedConfiguration { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is pattern instance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is pattern instance; otherwise, <c>false</c>.
        /// </value>
        bool IsPatternInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is speed pak.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is speed pak; otherwise, <c>false</c>.
        /// </value>
        bool IsSpeedPak { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is envelope.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is envelope; otherwise, <c>false</c>.
        /// </value>
        bool IsEnvelope { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [excluded from bom].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [excluded from bom]; otherwise, <c>false</c>.
        /// </value>
        bool ExcludedFromBOM { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is smart component.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is smart component; otherwise, <c>false</c>.
        /// </value>
        bool IsSmartComponent { get; set; }

        /// <summary>
        /// Gets or sets the unsafe object.
        /// </summary>
        /// <value>
        /// The unsafe object.
        /// </value>
        dynamic UnsafeObject { get; set; }

        /// <summary>
        /// Gets the name relative to.
        /// </summary>
        /// <param name="rootComponent">The root component.</param>
        /// <returns></returns>
        string GetNameRelativeTo(IComponent rootComponent);

        /// <summary>
        /// Gets the selection relative to.
        /// </summary>
        /// <param name="rootComponent">The root component.</param>
        /// <returns></returns>
        string GetSelectionRelativeTo(IComponent rootComponent);
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        bool Initialize();
    }
}

 