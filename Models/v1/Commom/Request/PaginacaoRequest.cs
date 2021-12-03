using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.Commom.Request
{
    public class PaginacaoRequest
    {
        /// <summary>
        /// Tamanho da pagina, 0 é igual a todos os items.
        /// </summary>
        /// <value></value>
        [Range(0, 500)]
        public int Tamanho { get; set; } = 0;
        /// <summary>
        /// Número da pagina
        /// </summary>
        /// <value></value>
        [Range(1, int.MaxValue)]
        public int Pagina { get; set; } = 1;
    }
}