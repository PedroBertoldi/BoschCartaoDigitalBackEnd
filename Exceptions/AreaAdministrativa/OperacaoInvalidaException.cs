using System;
using System.Runtime.Serialization;

namespace BoschCartaoDigitalBackEnd.Exceptions.AreaAdministrativa
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