using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.Request.AreaAdmin
{
    public class ListarBeneficiosEventoRequest
    {
        /// <summary>
        /// Id do evento a ser consultado.
        /// </summary>
        [Required]
        public int? EventoId { get; set; }

    }
}