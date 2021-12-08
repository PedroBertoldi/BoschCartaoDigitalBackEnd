using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("permissao")]
    public partial class Permissao
    {
        [Key]
        [Column("colaboradorID")]
        public int ColaboradorId { get; set; }
        [Key]
        [Column("tipoPermissaoID")]
        public int TipoPermissaoId { get; set; }

        [ForeignKey(nameof(ColaboradorId))]
        [InverseProperty("Permissao")]
        public virtual Colaborador Colaborador { get; set; }
        [ForeignKey(nameof(TipoPermissaoId))]
        [InverseProperty("Permissao")]
        public virtual TipoPermissao TipoPermissao { get; set; }
    }
}
