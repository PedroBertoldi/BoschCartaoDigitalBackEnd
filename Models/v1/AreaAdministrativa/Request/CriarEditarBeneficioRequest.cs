using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class CriarEditarBeneficioRequest
    {
        /// <summary>
        /// Nome do beneficio.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Beneficio { get; set; }
    }
}