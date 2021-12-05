using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa
{
    public class DireitosTodosColaboradoresAgrupadosResponse
    {
        public virtual EventoResponse Evento { get; set; }

        public List<DireitosColaboradorAgrupadosSemEventoResponse> ColaboradoresDireitos { get; set; }

    }
}