using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.CustomProperties;
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
        /// Sets the specified custom property in the specified document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        void Set(IDocument doc, string propertyName, object value);


        /// <summary>
        /// Deletes the specified property in the specified document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        void Delete(IDocument doc, string propertyName, string configurationName = "");
    }  
}
