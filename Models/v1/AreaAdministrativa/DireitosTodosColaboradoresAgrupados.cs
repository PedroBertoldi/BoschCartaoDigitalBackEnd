using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa
{
    public class DireitosTodosColaboradoresAgrupados
    {
        public virtual Evento Evento { get; set; }

        public List<DireitosColaboradorAgrupadosSemEvento> ColaboradoresDireitos { get; set; }

    }
}