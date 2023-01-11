using BlueByte.SOLIDWORKS.SDK.Core;
using System;
using System.Runtime.Serialization;

namespace BlueByte.SOLIDWORKS.SDK.Exceptions
{
    /// <summary>
    /// Identity exception. This exception occurs when <see cref="AddInBase"/> fails to read the identity attributes.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class IdentityInfoException : Exception
    
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityInfoException"/> class.
        /// </summary>
        public IdentityInfoException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityInfoException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public IdentityInfoException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityInfoException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public IdentityInfoException(string message, Exception innerException) : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityInfoException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected IdentityInfoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}