using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response
{
    public class DireitosPorColaboradorAgrupadosResponse
    {
        public ColaboradorResponse Colaborador { get; set; }
        public List<DireitoInfoReduzidaResponse> Direitos { get; set; }
        public virtual EventoResponse Evento { get; set; }
        public List<DireitosPorColaboradorAgrupadosResponse> Indicacoes {get;set;}
    }
}