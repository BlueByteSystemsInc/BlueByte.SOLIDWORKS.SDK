using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes.Menus
{
    /// <summary>
    /// SOLIDWORKS Menu.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MenuItemAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemAttribute" /> class. Add images to the project level and set their build action to embed resource. Size is 20x20, 32x32, 40x40, 64x64, 96x96 or 128x128.
        /// </summary>
        /// <param name="menuText">The menu text.</param>
        /// <param name="docTypes">The document types.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="bmpFileNameInResources">name of the bmp in the resources of the add-in project including the bmp extension.</param> 
        /// <remarks>Add images to the project level and set their build action to embed resource. Size is 20x20, 32x32, 40x40, 64x64, 96x96 or 128x128</remarks>
        public MenuItemAttribute(string menuText, DocumentTypes_e docTypes, string callback = "", string bmpFileNameInResources = "")
        {
            this.Text = menuText;
            this.DocumentType = docTypes;
            this.Callback = callback;
            this.BmpFileNameInResources = bmpFileNameInResources;
        }

        #region properties 

         

        /// <summary>
        /// Document type to which to add the menu item as defined by swDocumentTypes_e.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        public DocumentTypes_e DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the menu item. Menu item string ( e.g., "menuItem@menuString"); SOLIDWORKS creates menu items only if they do not already exist
        /// </summary>
        /// <value>
        /// The menu item.
        /// </value>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>
        /// <term>Note</term>
        /// </listheader>   
        /// <term>
        /// Specify the name of the menu for menuString (e.g., File, View, etc.) where you want your menu item to appear. If you do not specify menu string, then the menu item appears on the Tools menu below the Xpress Products menu item.
        /// </term>
        /// <term>
        /// Use the & symbol to include an accelerator key, e.g., MyItem@, adds MyItem to the File menu with an accelerator key. To display the accelerator key, press the Alt key. The accelerator key is underlined.
        /// </term>
        /// </list>
        /// </remarks>
        public string Text { get; set; }


        /// <summary>
        /// Gets or sets the position. Position where to add the new menu item; the first item is position 0; if -1 is specified for Position, then the new menu item is added to the bottom of the list; this argument specifies the position of the menu item in relation to its immediate parent menu.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position { get; set; } = -1;



        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        public string Callback { get; set; }

        /// <summary>
        /// Gets or sets the method that enables the state of the menu.
        /// </summary>
        /// <value>
        /// The state of the menu enable.
        /// </value>
        public string MenuEnableState { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>
        /// The hint.
        /// </value>
        public string Hint { get; set; }

        /// <summary>
        /// Gets or sets the image list. Array of strings for the paths for the image files for the menu item. Add your bmps to the resources of the add-in project. Add images to the project level and set their build action to embed resource. Size is 20x20, 32x32, 40x40, 64x64, 96x96 or 128x128.
        /// </summary>
        /// <value>
        /// The image list.
        /// </value>
        public string BmpFileNameInResources { get; set; }
        #endregion
    }
}
