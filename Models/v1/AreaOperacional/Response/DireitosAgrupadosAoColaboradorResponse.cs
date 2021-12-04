using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Response
{
    public class DireitosAgrupadosAoColaboradorResponse
    {
        public EventoResponse Evento { get; set; }
        public List<DireitoResponseResumido> DireitosColaborador { get; set; }
        public ColaboradorResponseResumida Colaborador { get; set; }
        public List<DireitosAgrupadosAoColaboradorResponse> SolicitadoParaRetirar { get; set; }
    }
}