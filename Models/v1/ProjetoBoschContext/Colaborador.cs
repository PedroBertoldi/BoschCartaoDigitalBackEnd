using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    [Table("colaborador")]
    public partial class Colaborador
    {
        public Colaborador()
        {
            DireitoColaborador = new HashSet<Direito>();
            DireitoIndicado = new HashSet<Direito>();
            DireitoRetirado = new HashSet<Direito>();
            InverseOrigem = new HashSet<Colaborador>();
            Permissao = new HashSet<Permissao>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("cpf")]
        [StringLength(11)]
        public string Cpf { get; set; }
        [Column("nomeCompleto")]
        [StringLength(255)]
        public string NomeCompleto { get; set; }
        [Column("dataNascimento", TypeName = "date")]
        public DateTime? DataNascimento { get; set; }
        [Column("unidadeOrganizacionalID")]
        public int? UnidadeOrganizacionalId { get; set; }
        [Column("EDV")]
        [StringLength(10)]
        public string Edv { get; set; }
        [StringLength(15)]
        public string Senha { get; set; }
        [Column("origemID")]
        public int? OrigemId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataDeCadastro { get; set; }

        [ForeignKey(nameof(OrigemId))]
        [InverseProperty(nameof(Colaborador.InverseOrigem))]
        public virtual Colaborador Origem { get; set; }
        [ForeignKey(nameof(UnidadeOrganizacionalId))]
        [InverseProperty("Colaborador")]
        public virtual UnidadeOrganizacional UnidadeOrganizacional { get; set; }
        [InverseProperty(nameof(Direito.Colaborador))]
        public virtual ICollection<Direito> DireitoColaborador { get; set; }
        [InverseProperty(nameof(Direito.Indicado))]
        public virtual ICollection<Direito> DireitoIndicado { get; set; }
        [InverseProperty(nameof(Direito.Retirado))]
        public virtual ICollection<Direito> DireitoRetirado { get; set; }
        [InverseProperty(nameof(Colaborador.Origem))]
        public virtual ICollection<Colaborador> InverseOrigem { get; set; }
        [InverseProperty("Colaborador")]
        public virtual ICollection<Permissao> Permissao { get; set; }
    }
}
