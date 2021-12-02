using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("beneficio")]
    public partial class Beneficio
    {
        public Beneficio()
        {
            BeneficioEvento = new HashSet<BeneficioEvento>();
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

        [InverseProperty("Beneficio")]
        public virtual ICollection<BeneficioEvento> BeneficioEvento { get; set; }
        [InverseProperty("Beneficio")]
        public virtual ICollection<Direito> Direito { get; set; }
    }
}
