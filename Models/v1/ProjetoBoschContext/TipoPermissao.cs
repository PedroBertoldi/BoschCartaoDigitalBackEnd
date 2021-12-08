using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("tipoPermissao")]
    public partial class TipoPermissao
    {
        public TipoPermissao()
        {
            Permissao = new HashSet<Permissao>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("descricao")]
        [StringLength(255)]
        public string Descricao { get; set; }
        [Column("descricaoNormalizada")]
        [StringLength(255)]
        public string DescricaoNormalizada { get; set; }

        [InverseProperty("TipoPermissao")]
        public virtual ICollection<Permissao> Permissao { get; set; }
    }
}
