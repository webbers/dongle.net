using System;
using System.Runtime.Serialization;

namespace WebUtils.Mvc.Authentication
{
    [Serializable]
    public class BadUsernameOrPasswordException : ApplicationException
    {
        public BadUsernameOrPasswordException()
        {
        }

        public BadUsernameOrPasswordException(string message) : base(message)
        {
        }

        public BadUsernameOrPasswordException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BadUsernameOrPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
