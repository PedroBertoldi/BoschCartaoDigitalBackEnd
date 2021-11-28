using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("unidadeOrganizacional")]
    public partial class UnidadeOrganizacional
    {
        public UnidadeOrganizacional()
        {
            Colaborador = new HashSet<Colaborador>();
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

        [InverseProperty("UnidadeOrganizacional")]
        public virtual ICollection<Colaborador> Colaborador { get; set; }
    }
}
