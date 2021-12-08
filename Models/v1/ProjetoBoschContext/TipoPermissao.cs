using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class TipoPermissao
    {
        public TipoPermissao()
        {
            Permissao = new HashSet<Permissao>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string DescricaoNormalizada { get; set; }

        public virtual ICollection<Permissao> Permissao { get; set; }
    }
}
