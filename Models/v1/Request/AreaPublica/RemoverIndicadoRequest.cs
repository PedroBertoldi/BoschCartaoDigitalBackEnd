using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica
{
    public class RemoverIndicadoRequest
    {
        /// <summary>
        /// Id do evento no qual sera solicitado a remoção, caso nulo será o evento ativo
        /// </summary>
        public int? EventoId { get; set; }
        /// <summary>
        /// Id do colaborador no qual os direitos terão suas indicações removidas.
        /// </summary>
        [Required]
        public int? ColaboradorId { get; set; }
        /// <summary>
        /// Ids dos direitos para remoção de indicação
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public List<long> DireitosId { get; set; }
    }
}