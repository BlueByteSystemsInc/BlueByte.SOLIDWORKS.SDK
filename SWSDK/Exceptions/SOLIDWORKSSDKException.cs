using System;
using System.Runtime.Serialization;

namespace BlueByte.SOLIDWORKS.SDK.Exceptions
{
    [Serializable]
    internal class SOLIDWORKSSDKException : Exception
    {
        public SOLIDWORKSSDKException()
        {
        }

        public SOLIDWORKSSDKException(string message) : base(message)
        {
        }

        public SOLIDWORKSSDKException(string message, Exception innerException) : base(message, innerException)
        {
        }

        

        protected SOLIDWORKSSDKException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}