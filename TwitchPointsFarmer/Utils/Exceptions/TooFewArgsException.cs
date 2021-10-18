using System;
using System.Runtime.Serialization;

namespace TwitchPointsFarmer.Utils.Exceptions
{
    [Serializable]
    internal class TooFewArgsException : Exception
    {
        public TooFewArgsException()
        {
        }

        public TooFewArgsException(string message) : base(message)
        {
        }

        public TooFewArgsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooFewArgsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}