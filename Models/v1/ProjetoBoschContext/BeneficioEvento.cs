using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("beneficioEvento")]
    public partial class BeneficioEvento
    {
        [Key]
        [Column("beneficioID")]
        public int BeneficioId { get; set; }
        [Key]
        [Column("eventoID")]
        public int EventoId { get; set; }

        [ForeignKey(nameof(BeneficioId))]
        [InverseProperty("BeneficioEvento")]
        public virtual Beneficio Beneficio { get; set; }
        [ForeignKey(nameof(EventoId))]
        [InverseProperty("BeneficioEvento")]
        public virtual Evento Evento { get; set; }
    }
}
