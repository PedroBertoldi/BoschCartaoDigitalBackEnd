using System;
using System.Collections.Generic;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext
{
    public partial class Direito
    {
        public long Id { get; set; }
        public int ColaboradorId { get; set; }
        public int EventoId { get; set; }
        public int? BeneficioId { get; set; }
        public int? IndicadoId { get; set; }
        public int? RetiradoId { get; set; }
        public DateTime? DataRetirada { get; set; }

        public virtual Beneficio Beneficio { get; set; }
        public virtual Colaborador Colaborador { get; set; }
        public virtual Evento Evento { get; set; }
        public virtual Colaborador Indicado { get; set; }
        public virtual Colaborador Retirado { get; set; }
    }
}
