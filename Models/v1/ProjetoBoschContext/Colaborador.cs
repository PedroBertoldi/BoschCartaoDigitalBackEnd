using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
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

        public int Id { get; set; }
        public string Cpf { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? UnidadeOrganizacionalId { get; set; }
        public string Edv { get; set; }
        public string Senha { get; set; }
        public int? OrigemId { get; set; }
        public DateTime? DataDeCadastro { get; set; }

        public virtual Colaborador Origem { get; set; }
        public virtual UnidadeOrganizacional UnidadeOrganizacional { get; set; }
        public virtual ICollection<Direito> DireitoColaborador { get; set; }
        public virtual ICollection<Direito> DireitoIndicado { get; set; }
        public virtual ICollection<Direito> DireitoRetirado { get; set; }
        public virtual ICollection<Colaborador> InverseOrigem { get; set; }
        public virtual ICollection<Permissao> Permissao { get; set; }
    }
}
