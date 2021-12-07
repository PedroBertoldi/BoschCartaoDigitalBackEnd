using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional
{
    public class ColaboradoresAgrupadosAoDireito
    {
        public Beneficio beneficio { get; set; }
        public List<BeneficiarioResumido> beneficiarios { get; set; }
    }
}