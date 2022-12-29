using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{
    /// <summary>
    /// Component removed event 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ComponentRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the component.
        /// </summary>
        /// <value>
        /// The component.
        /// </value>
        public IComponent Component { get; set; }

        /// <summary>
        /// News this class.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static ComponentRemovedEventArgs New(IComponent e)
        {
            var instance = new ComponentRemovedEventArgs();

            instance.Component = e;

            return instance;
        } 
    }
}

 