using System;
using System.ComponentModel.DataAnnotations;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request
{
    public class CriarEditarEventoRequest
    {
        /// <summary>
        /// Nome do evento.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string NomeEvento { get; set; }
        /// <summary>
        /// Descrição do evento.
        /// </summary>
        [MaxLength(255)]
        public string Descricao { get; set; }
        /// <summary>
        /// Data e hora de inicio do evento
        /// </summary>
        [Required]
        public DateTime? Inicio { get; set; }
        /// <summary>
        /// Data e hora do fim do evento
        /// </summary>
        [Required]
        public DateTime? Fim { get; set; }
    }
}