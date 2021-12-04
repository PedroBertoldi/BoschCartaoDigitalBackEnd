using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Request
{
    public class DireitoEntregueRequest
    {
        /// <summary>
        /// Id do evento tratado, se nulo será utilizado o proximo evento ativo ou registado.
        /// </summary>
        public int? EventoId { get; set; }
        /// <summary>
        /// Id do colaborador que retirou o beneficio.
        /// </summary>
        public int? ColaboradorId { get; set; }
        /// <summary>
        /// CPF da pessoa que recebeu, caso terceiros.
        /// </summary>
        [MaxLength(11)]
        public string CpfRecebedor { get; set; }
        /// <summary>
        /// Direitos que foram entregues
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public List<long> DireitosEntregues { get; set; }
    }
}