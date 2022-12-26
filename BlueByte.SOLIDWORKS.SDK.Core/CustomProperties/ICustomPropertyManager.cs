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

    }
}
