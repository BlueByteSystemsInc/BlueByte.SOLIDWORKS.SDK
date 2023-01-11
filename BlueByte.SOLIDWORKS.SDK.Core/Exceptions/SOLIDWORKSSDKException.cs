using System;
using System.Runtime.Serialization;

namespace BlueByte.SOLIDWORKS.SDK.Exceptions
{
    /// <summary>
    /// Exception throwing by the Blue Byte Systems SOLIDWORKS SDK.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class SOLIDWORKSSDKException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SOLIDWORKSSDKException"/> class.
        /// </summary>
        public SOLIDWORKSSDKException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SOLIDWORKSSDKException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SOLIDWORKSSDKException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SOLIDWORKSSDKException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public SOLIDWORKSSDKException(string message, Exception innerException) : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SOLIDWORKSSDKException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected SOLIDWORKSSDKException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}