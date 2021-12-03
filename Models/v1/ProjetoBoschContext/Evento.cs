using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("evento")]
    public partial class Evento
    {
        public Evento()
        {
            BeneficioEvento = new HashSet<BeneficioEvento>();
            Direito = new HashSet<Direito>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("nome")]
        [StringLength(50)]
        public string Nome { get; set; }
        [Column("descricao")]
        [StringLength(255)]
        public string Descricao { get; set; }
        [Column("descricaoNormalizada")]
        [StringLength(255)]
        public string DescricaoNormalizada { get; set; }
        [Column("dataInicio", TypeName = "datetime")]
        public DateTime? DataInicio { get; set; }
        [Column("dataFim", TypeName = "datetime")]
        public DateTime? DataFim { get; set; }

        [InverseProperty("Evento")]
        public virtual ICollection<BeneficioEvento> BeneficioEvento { get; set; }
        [InverseProperty("Evento")]
        public virtual ICollection<Direito> Direito { get; set; }
    }
}
