using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    /// <summary>
    /// Enables or disables add-in during SOLIDWORKS startup.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class StartUp : Attribute
    {

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="StartUp"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUp"/> class.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public StartUp(bool enabled)
        {
            this.Enabled = enabled;
        }
    }
}
