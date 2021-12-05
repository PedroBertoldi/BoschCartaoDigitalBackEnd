using System;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class DireitosColaboradorRequest
    {
        /// <summary>
        /// Id do evento para procurar os beneficios disponíveis, se nulo retorna o primeiro evento ativo.
        /// </summary>
        [Required]
        public int? EventoID { get; set; }
        /// <summary>
        /// id da pessoa no qual os beneficios serão consultados.
        /// </summary>
        /// <value></value>
        [Required]
        public int? idColaborador { get; set; }
    }
}