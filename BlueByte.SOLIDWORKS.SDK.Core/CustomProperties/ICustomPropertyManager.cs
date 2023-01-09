using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.CustomProperties
{
    /// <summary>
    /// Custom property manager
    /// </summary>
    public interface ICustomPropertyManager : IDisposable
    {

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



        /// <summary>
        /// Gets the property names from the specified configuration. Get custom properties if <paramref name="configurationName"/> is ignored. 
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <returns>Array of property names</returns>
        string[] GetNames(IDocument doc, string configurationName = "");



        /// <summary>
        /// Sets the specified custom property in the specified document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="configurationName"></param>
        swCustomInfoSetResult_e Set(IDocument doc, string propertyName, object value, string configurationName = "");


        /// <summary>
        /// Deletes the specified property in the specified document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        void Delete(IDocument doc, string propertyName, string configurationName = "");


        /// <summary>
        /// Adds the specified property in the specified document safely.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        swCustomInfoAddResult_e AddSafe(IDocument doc, string propertyName, object value, swCustomInfoType_e dataType = swCustomInfoType_e.swCustomInfoText, string configurationName = "");
        

            /// <summary>
            /// Initializes this instance.
            /// </summary>
            void Initialize();
    }  
}
