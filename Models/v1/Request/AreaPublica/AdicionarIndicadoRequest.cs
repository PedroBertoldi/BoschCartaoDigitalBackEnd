using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica
{
    public class AdicionarIndicadoRequest
    {
        /// <summary>
        /// Id do colaborador solicitando a indicação.
        /// </summary>
        [Required]
        public int? ColaboradorId { get; set; }
        /// <summary>
        /// Nome do indicado, caso não exista será cadastrado um novo.
        /// </summary>
        /// <value></value>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string NomeCompleto { get; set; }
        /// <summary>
        /// cpf do indicado, nulo se edv informado.
        /// </summary>
        /// <value></value>
        [MaxLength(11)]
        public string Cpf { get; set; }
        /// <summary>
        /// Edv do indicado, nulo se cpf informado.
        /// </summary>
        /// <value></value>
        public string Edv { get; set; }
        /// <summary>
        /// Lista com id dos direitos os quais se deseja atribuir a indicação.
        /// </summary>
        /// <value></value>
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public List<int> DireitosId { get; set; }
    }
}