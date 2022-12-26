using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.CustomProperties
{

    /// <summary>
    /// Encapsulates the changes of a custom property.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class CustomPropertyChangedEventArgs : EventArgs
    {
        #region properties         
        /// <summary>
        /// Gets the type of the change.
        /// </summary>
        /// <value>
        /// The type of the change.
        /// </value>
        public ChangeType ChangeType { get; private set; }
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public IDocument Document { get; private set; }
        /// <summary>
        /// Gets the name of the configuration.
        /// </summary>
        /// <value>
        /// The name of the configuration.
        /// </value>
        public string ConfigurationName { get; private set; }
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }
        /// <summary>
        /// gets the new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public string NewValue { get; private set; }
        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public string OldValue { get; private set; }
        #endregion 


        internal static CustomPropertyChangedEventArgs New(ChangeType changeType, IDocument doc, string propertyName, string configurationName = "", string oldValue = null, string newValue = null)
        {
            var instance = new CustomPropertyChangedEventArgs();

            instance.ChangeType = changeType;
            instance.PropertyName = propertyName;
            instance.Document = doc;
            instance.ConfigurationName = configurationName;
            instance.OldValue = oldValue;
            instance.NewValue = newValue;

            return instance;
        }

        private CustomPropertyChangedEventArgs()
        {

        }
    }


  
}
