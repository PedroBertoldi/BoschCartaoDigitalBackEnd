using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class ListarBeneficiosEventoRequest
    {
        /// <summary>
        /// Id do evento a ser consultado.
        /// </summary>
        [Required]
        public int? eventoId { get; set; }

    }
}