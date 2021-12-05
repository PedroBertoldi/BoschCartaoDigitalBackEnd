using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class DireitoRequestResumido
    {
        /// <summary>
        /// Id do evento.
        /// </summary>
        [Required]
        public int? EventoId { get; set; }

        /// <summary>
        /// Id do beneficio.
        /// </summary>
        [Required]
        public int? BeneficioID { get; set; }

        /// <summary>
        /// Quantidade deste beneficio.
        /// </summary>
        [Required]
        public int? QtdBeneficio { get; set; }

    }
}