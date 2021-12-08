using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class Beneficio
    {
        public Beneficio()
        {
            BeneficioEvento = new HashSet<BeneficioEvento>();
            Direito = new HashSet<Direito>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string DescricaoNormalizada { get; set; }

        public virtual ICollection<BeneficioEvento> BeneficioEvento { get; set; }
        public virtual ICollection<Direito> Direito { get; set; }
    }
}
