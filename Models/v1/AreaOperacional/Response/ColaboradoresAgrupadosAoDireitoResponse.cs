using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional
{
    public class ColaboradoresAgrupadosAoDireitoResponse
    {
        public BeneficioResponse beneficio { get; set; }
        public List<BeneficiarioResumidoResponse> beneficiarios { get; set; }
    }
}