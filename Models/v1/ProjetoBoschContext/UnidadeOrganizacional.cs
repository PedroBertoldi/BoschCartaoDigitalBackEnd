using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class UnidadeOrganizacional
    {
        public UnidadeOrganizacional()
        {
            Colaborador = new HashSet<Colaborador>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string DescricaoNormalizada { get; set; }

        public virtual ICollection<Colaborador> Colaborador { get; set; }
    }
}
