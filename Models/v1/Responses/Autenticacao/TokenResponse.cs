using System;
using System.Collections.Generic;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.Autenticacao
{
    public class TokenResponse
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string NomeCompleto { get; set; }
        /// <summary>
        /// Id do usuário no banco de dados.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Lista de permissões/cargos do usuário.
        /// </summary>
        public List<string> Cargos { get; set; }
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