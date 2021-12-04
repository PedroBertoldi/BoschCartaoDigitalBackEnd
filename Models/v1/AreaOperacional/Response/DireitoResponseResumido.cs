using System;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Response
{
    public class DireitoResponseResumido
    {
        public long Id { get; set; }
        public DateTime? DataRetirada { get; set; }
        public virtual BeneficioResponse Beneficio { get; set; }
        public virtual ColaboradorResponse Retirado { get; set; }
    }
}