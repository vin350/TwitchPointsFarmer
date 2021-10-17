using System;
using System.Runtime.Serialization;

namespace TwitchPointsFarmer.Utils.Exceptions
{
    [Serializable]
    internal class TooManyArgsException : Exception
    {
        public TooManyArgsException()
        {
        }

        public TooManyArgsException(string message) : base(message)
        {
        }

        public TooManyArgsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooManyArgsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}