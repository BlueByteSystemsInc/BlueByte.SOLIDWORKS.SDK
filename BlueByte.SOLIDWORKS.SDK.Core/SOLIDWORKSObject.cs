using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// Base wrapper class for SOLIDWORKS object.
    /// </summary>
    /// <seealso cref="BlueByte.SOLIDWORKS.SDK.Core.ISOLIDWORKSObject" />
    public abstract class SOLIDWORKSObject : ISOLIDWORKSObject
    {
        /// <summary>
        /// Gets or sets the unsafe object.
        /// </summary>
        /// <value>
        /// The un safe object.
        /// </value>
        public dynamic UnSafeObject { get; set; }

        /// <summary>
        /// Casts this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T As<T>()
        {
            return (T)UnSafeObject;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        internal void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        internal void Flush()
        {
            PropertyChanged = null;
        }
    }
}