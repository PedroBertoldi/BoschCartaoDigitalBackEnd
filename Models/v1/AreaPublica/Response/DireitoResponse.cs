using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response
{
    public class DireitoResponse
    {
        public long Id { get; set; }
        public DateTime? DataRetirada { get; set; }
        public virtual BeneficioResponse Beneficio { get; set; }
        public virtual ColaboradorResponse Colaborador { get; set; }
        public virtual EventoResponse Evento { get; set; }
        public virtual ColaboradorResponse Indicado { get; set; }
        public virtual ColaboradorResponse Retirado { get; set; }
    }
}