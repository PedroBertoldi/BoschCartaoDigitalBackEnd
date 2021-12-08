using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class Evento
    {
        public Evento()
        {
            BeneficioEvento = new HashSet<BeneficioEvento>();
            Direito = new HashSet<Direito>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string DescricaoNormalizada { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public virtual ICollection<BeneficioEvento> BeneficioEvento { get; set; }
        public virtual ICollection<Direito> Direito { get; set; }
    }
}
