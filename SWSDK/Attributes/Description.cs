using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    /// <summary>
    /// Add-in description.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class Description : Attribute
    {

        /// <summary>
        /// Gets or sets the add in description.
        /// </summary>
        /// <value>
        /// The add in description.
        /// </value>
        public string AddInDescription { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        /// <param name="Description">The description.</param>
        public Description(string Description)
        {
            this.AddInDescription = Description;
        }
    }
}
