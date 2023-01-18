using BlueByte.SOLIDWORKS.SDK.Core.CustomProperties;
using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    /// <summary>
    /// Document object.
    /// </summary>
    /// <seealso cref="BlueByte.SOLIDWORKS.SDK.Core.ISOLIDWORKSObject" />
    /// <seealso cref="System.IDisposable" />
    public interface IDocument : ISOLIDWORKSObject, IDisposable
    {
        #region events 

        

        /// <summary>
        /// Occurs when [custom property changed].
        /// </summary>
        event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyChanged;
        /// <summary>
        /// Occurs when [custom property added].
        /// </summary>
        event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyAdded;
        /// <summary>
        /// Occurs when [custom property deleted].
        /// </summary>
        event EventHandler<CustomPropertyChangedEventArgs> CustomPropertyDeleted;

        #endregion


        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        swDocumentTypes_e DocumentType { get;   }

        /// <summary>
        /// Gets the custom property manager.
        /// </summary>
        /// <value>
        /// The custom property manager.
        /// </value>
        ICustomPropertyManager CustomPropertyManager { get; set; }

        /// <summary>
        /// Gets a value indicating whether this document is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this document is visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        string FileName { get;   }


        


        /// <summary>
        /// Gets or sets the name of the active configuration.
        /// </summary>
        /// <value>
        /// The name of the active configuration.
        /// </value>
        string ActiveConfigurationName { get; set; }

        /// <summary>
        /// Gets a value indicating whether this document is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this document is loaded; otherwise, <c>false</c>.
        /// </value>
        bool IsLoaded { get;   }

        /// <summary>
        /// Occurs when [got closed].
        /// </summary>
        event EventHandler<swDestroyNotifyType_e> GotClosed;

        /// <summary>
        /// Occurs when [before saved as].
        /// </summary>
        event EventHandler<SaveEventArgs> BeforeSavedAs;

        /// <summary>
        /// Attaches the event handlers.
        /// </summary>
        void AttachEventHandlers();

        /// <summary>
        /// Loads the specified unsafe object.
        /// </summary>
        /// <param name="UnsafeObject">The unsafe object.</param>
        void Load(object UnsafeObject);

        /// <summary>
        /// Dettaches the event handlers.
        /// </summary>
        void DettachEventHandlers();

        /// <summary>
        /// Refreshes this document.
        /// </summary>
        void Refresh();


        /// <summary>
        /// Equalses the specified document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns></returns>
        bool Equals(IDocument doc);


        /// <summary>
        /// Equalses the specified document.
        /// </summary>
        /// <param name="filename">filename.</param>
        /// <returns></returns>
        bool Equals(string filename);

        /// <summary>
        /// Shows the specified configuration.
        /// </summary>
        void ShowConfiguration(string configurationName);



        /// <summary>
        /// Saves the current document. True if saved, false if not. 
        /// </summary>
        /// <param name="extensions">Extensions. Enum supports flags.</param>
        bool Save(FileExtension_e extensions = FileExtension_e.Default);
    }


    
}