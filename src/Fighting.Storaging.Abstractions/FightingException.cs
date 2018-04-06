using System;
using System.Runtime.Serialization;

namespace Fighting.Storaging
{
    /// <summary>
    /// Base exception type for those are thrown by Abp system for Abp specific exceptions.
    /// </summary>
    [Serializable]
    public class FightingException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="FightingException"/> object.
        /// </summary>
        public FightingException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="FightingException"/> object.
        /// </summary>
        public FightingException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="FightingException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public FightingException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="FightingException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public FightingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
