using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class CriarEditarBeneficiarioDireitoRequestResumido
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
        public int? BeneficioId { get; set; }

        /// <summary>
        /// Quantidade deste beneficio.
        /// </summary>
        [Required]
        public int? QtdBeneficio { get; set; }

    }
}