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
            Direito = new HashSet<Direito>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("descricao")]
        [StringLength(255)]
        public string Descricao { get; set; }
        [Column("descricaNormalizada")]
        [StringLength(255)]
        public string DescricaNormalizada { get; set; }
        [Column("dataInicio", TypeName = "datetime")]
        public DateTime? DataInicio { get; set; }
        [Column("dataFim", TypeName = "datetime")]
        public DateTime? DataFim { get; set; }

        [InverseProperty("Evento")]
        public virtual ICollection<Direito> Direito { get; set; }
    }
}
