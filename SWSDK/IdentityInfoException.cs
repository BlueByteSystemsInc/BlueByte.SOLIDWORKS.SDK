using System;
using System.Runtime.Serialization;

namespace SWSDK
{
    [Serializable]
    public class IdentityInfoException : Exception
    {
        public IdentityInfoException()
        {
        }

        public IdentityInfoException(string message) : base(message)
        {
        }

        public IdentityInfoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        

        protected IdentityInfoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}