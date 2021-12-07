using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional
{
    public class BeneficiarioResumidoResponse
    {
        public int idColaborador { get; set; }
        public string nomeCompleto { get; set; }
        public long idDireito { get; set; }
    }
}