using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    /// <summary>
    /// Add-in name.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the add in.
        /// </summary>
        /// <value>
        /// The name of the add in.
        /// </value>
        public string AddInName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public NameAttribute(string name)
        {
            this.AddInName = name;
        }
    }
}
