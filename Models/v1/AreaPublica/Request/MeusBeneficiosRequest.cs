using System;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Request
{
    public class MeusBeneficiosRequest
    {
        /// <summary>
        /// Id do evento para procurar os beneficios disponíveis, se nulo retorna o primeiro evento ativo.
        /// </summary>
        public int? EventoID { get; set; }
        /// <summary>
        /// CPF da pessoa no qual os beneficios serão consultados.
        /// </summary>
        /// <value></value>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(11)]
        public string Cpf { get; set; }
        /// <summary>
        /// Data de nascimento da pessoa no qual os beneficios serão consultados.
        /// </summary>
        /// <value></value>
        [Required]
        public DateTime? DataNascimento { get; set; }
    }
}