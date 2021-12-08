using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class BeneficioEvento
    {
        public int BeneficioId { get; set; }
        public int EventoId { get; set; }

        public virtual Beneficio Beneficio { get; set; }
        public virtual Evento Evento { get; set; }
    }
}
