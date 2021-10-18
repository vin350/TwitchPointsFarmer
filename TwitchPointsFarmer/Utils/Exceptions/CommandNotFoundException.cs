using System;
using System.Runtime.Serialization;

namespace TwitchPointsFarmer.Utils.Exceptions
{
    [Serializable]
    internal class CommandNotFoundException : Exception
    {
        public CommandNotFoundException()
        {
        }

        public CommandNotFoundException(string message) : base(message)
        {
        }

        public CommandNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}