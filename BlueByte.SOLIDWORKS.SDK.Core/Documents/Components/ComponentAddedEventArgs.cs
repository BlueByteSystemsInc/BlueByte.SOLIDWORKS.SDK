using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{
    public class ComponentAddedEventArgs : EventArgs
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
        public static ComponentAddedEventArgs New(IComponent e)
        {
            var instance = new ComponentAddedEventArgs();

            instance.Component = e;

            return instance;
        }
    }
}

 