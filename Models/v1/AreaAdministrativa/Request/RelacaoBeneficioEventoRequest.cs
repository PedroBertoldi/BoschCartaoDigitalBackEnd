using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class RelacaoBeneficioEventoRequest
    {
        /// <summary>
        /// Id do evento a ser cadastrado
        /// </summary>
        [Required]
        public int? EventoId { get; set; }
        /// <summary>
        /// Id do beneficio a ser cadastrado
        /// </summary>
        /// <value></value>
        [Required]
        public int? BeneficioId { get; set; }
    }
}