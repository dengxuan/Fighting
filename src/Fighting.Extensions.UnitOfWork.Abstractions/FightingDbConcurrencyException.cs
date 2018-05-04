using Fighting.Storaging;
using System;
using System.Runtime.Serialization;

namespace Fighting.Extensions.UnitOfWork
{
    [Serializable]
    public class FightingDbConcurrencyException : FightingException
    {
        /// <summary>
        /// Creates a new <see cref="FightingDbConcurrencyException"/> object.
        /// </summary>
        public FightingDbConcurrencyException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public FightingDbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="FightingDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public FightingDbConcurrencyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="FightingDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public FightingDbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}