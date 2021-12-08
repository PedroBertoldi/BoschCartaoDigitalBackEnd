using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class Permissao
    {
        public int ColaboradorId { get; set; }
        public int TipoPermissaoId { get; set; }

        public virtual Colaborador Colaborador { get; set; }
        public virtual TipoPermissao TipoPermissao { get; set; }
    }
}
