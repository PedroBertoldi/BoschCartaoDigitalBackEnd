using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.Autenticacao
{
    public class TokenResponse
    {
        /// <summary>
        /// Token gerado.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Data de validade do token.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}