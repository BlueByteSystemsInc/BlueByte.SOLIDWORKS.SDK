using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes.Menus
{
    /// <summary>
    /// SOLIDWORKS PopupMenu. This menu appears when you right-clicks on an entity in the 3D area or the feature tree.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PopMenuItemAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopMenuItemAttribute" /> class.
        /// </summary>
        /// <param name="menuText">The menu text.</param>
        /// <param name="docTypes">The document types.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="customNames">Custom names for selection types. Semi-colon separated list of the names of the custom feature types; this argument is applicable only if SelectType is a custom feature type (like swSelATTRIBUTES); in the case of swSelATTRIBUTES, set this field to the name of the attribute definition</param>
        public PopMenuItemAttribute(string menuText, swDocumentTypes_e docTypes, string callback = "", string customNames = null)
        {
            this.Text = menuText;
            this.DocumentType = docTypes;
            this.Callback = callback;
            this.CustomNames = customNames;
 
           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopMenuItemAttribute" /> class.
        /// </summary>
        /// <param name="menuText">The menu text.</param>
        /// <param name="docTypes">The document types.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="menuEnable">Menu enable method name</param>
        /// <param name="type">Entity to select</param>
        /// <param name="customNames">customNames</param>
        public PopMenuItemAttribute(string menuText, swDocumentTypes_e docTypes, swSelectType_e selectionType, string callback = "", string hint = "", string menuEnable = "", string customNames = "")
        {
            this.Text = menuText;
            this.DocumentType = docTypes;
            this.Callback = callback;
            this.CustomNames = customNames;
            this.MenuEnableState = menuEnable;
            this.SelectionType = selectionType;
            this.Hint = hint;


        }

        #region properties 



        /// <summary>
        /// Document type to which to add the menu item as defined by swDocumentTypes_e.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        public swDocumentTypes_e DocumentType { get; set; }

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
        /// Specify the name of the menu for menuString (e.g., File, View, etc.) where you want your menu item to appear. If you do not specify menu string, then the menu item appears on the Tools menu below the X
        /// ss Products menu item.
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
        /// Gets or sets the custom names. Semi-colon separated list of the names of the custom feature types; this argument is applicable only if SelectType is a custom feature type (like swSelATTRIBUTES); in the case of swSelATTRIBUTES, set this field to the name of the attribute definition
        /// </summary>
        /// <value>
        /// The custom names.
        /// </value>
        public string CustomNames { get; set; }

        /// <summary>
        /// Gets or sets the type of the selection.
        /// </summary>
        /// <value>
        /// The type of the selection.
        /// </value>
        public swSelectType_e SelectionType { get; set; }

        #endregion


    }
}
