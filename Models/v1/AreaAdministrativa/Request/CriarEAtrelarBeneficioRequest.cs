using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class CriarEAtrelarBeneficioRequest : CriarEditarBeneficioRequest
    {
        /// <summary>
        /// Id do evento a ser atrelado ao beneficio
        /// </summary>
        [Required]
        public int? EventoId { get; set; }
    }
}