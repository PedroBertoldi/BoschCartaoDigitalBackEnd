using System;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Request
{
    public class BuscarIndicadoRequest
    {
        /// <summary>
        /// Id do evento para procurar os beneficios disponíveis, se nulo retorna o primeiro evento ativo.
        /// </summary>
        public int? EventoID { get; set; }
        
        /// <summary>
        /// id do titular dos benefícios
        /// </summary>
        /// <value></value>
        [Required]
        public int? ColaboradorId { get; set; }
        /// <summary>
        /// Data de nascimento do titular
        /// </summary>
        /// <value></value>
        [Required]
        public DateTime? DataNascimento { get; set; }
    }
}