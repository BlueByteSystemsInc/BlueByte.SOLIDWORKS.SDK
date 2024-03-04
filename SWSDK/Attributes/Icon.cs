using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    /// <summary>
    /// Icon file name.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class IconAttribute : Attribute
    {
 
        /// <summary>
        /// Initializes a new instance of the <see cref="IconAttribute"/> class.
        /// </summary>
        /// <param name="iconPath">icon Path.</param>
        public IconAttribute(string iconPath)
        {
            this.IconFileName = iconPath;
        }


        /// <summary>
        /// File name of the icon. The icon must be in the same folder as the assembly of your add-in. Size is 16x16
        /// </summary>
        /// <value>
        /// Full path to the icon.
        /// </value>
        public string IconFileName { get; set; }
    }
}
