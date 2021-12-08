using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("direito")]
    public partial class Direito
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("colaboradorID")]
        public int ColaboradorId { get; set; }
        [Column("eventoID")]
        public int EventoId { get; set; }
        [Column("beneficioID")]
        public int? BeneficioId { get; set; }
        [Column("indicadoID")]
        public int? IndicadoId { get; set; }
        [Column("retiradoID")]
        public int? RetiradoId { get; set; }
        [Column("dataRetirada", TypeName = "datetime")]
        public DateTime? DataRetirada { get; set; }

        [ForeignKey(nameof(BeneficioId))]
        [InverseProperty("Direito")]
        public virtual Beneficio Beneficio { get; set; }
        [ForeignKey(nameof(ColaboradorId))]
        [InverseProperty("DireitoColaborador")]
        public virtual Colaborador Colaborador { get; set; }
        [ForeignKey(nameof(EventoId))]
        [InverseProperty("Direito")]
        public virtual Evento Evento { get; set; }
        [ForeignKey(nameof(IndicadoId))]
        [InverseProperty("DireitoIndicado")]
        public virtual Colaborador Indicado { get; set; }
        [ForeignKey(nameof(RetiradoId))]
        [InverseProperty("DireitoRetirado")]
        public virtual Colaborador Retirado { get; set; }
    }
}
