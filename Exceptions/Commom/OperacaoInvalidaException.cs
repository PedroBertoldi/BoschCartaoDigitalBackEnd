using System;
using System.Runtime.Serialization;

namespace BoschCartaoDigitalBackEnd.Exceptions.Commom
{
    public class OperacaoInvalidaException : Exception
    {
        public OperacaoInvalidaException()
        {
        }

        public OperacaoInvalidaException(string message) : base(message)
        {
        }

        protected OperacaoInvalidaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}