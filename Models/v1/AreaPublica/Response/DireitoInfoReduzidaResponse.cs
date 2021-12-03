using System;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response
{
    public class DireitoInfoReduzidaResponse
    {
        public long Id { get; set; }
        public DateTime? DataRetirada { get; set; }
        public BeneficioResponse Beneficio { get; set; }
        public ColaboradorResponse Indicado { get; set; }
        public ColaboradorResponse Retirado { get; set; }
        
    }
}