using System;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.Request.Autenticacao
{
    public class LoginUserRequest
    {   
        /// <summary>
        /// CPF do usuário.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(11)]
        public string Cpf { get; set; }
        /// <summary>
        /// Data de nascimento do usuário, formato: yyyy-mm-dd
        /// </summary>
        [Required]
        public DateTime? DataNascimento { get; set; }
    }
}