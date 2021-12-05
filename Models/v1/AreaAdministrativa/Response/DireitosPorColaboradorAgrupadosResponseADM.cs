using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response
{
    public class DireitosPorColaboradorAgrupadosResponseADM
    {
        public virtual EventoResponse Evento { get; set; }

        public ColaboradorResponse Colaborador { get; set; }
        public ColaboradorResponse Indicado { get; set; }

        public List<DireitoInfoReduzidaResponseADM> Direitos { get; set; }
    }
}