using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.AreaPublica
{
    public class DireitosPorColaboradorAgrupadosResponse
    {
        public ColaboradorResponse Colaborador { get; set; }
        public List<DireitoInfoReduzidaResponse> Direitos { get; set; }
        public virtual EventoResponse Evento { get; set; }
        public List<DireitosPorColaboradorAgrupadosResponse> Indicacoes {get;set;}
    }
}