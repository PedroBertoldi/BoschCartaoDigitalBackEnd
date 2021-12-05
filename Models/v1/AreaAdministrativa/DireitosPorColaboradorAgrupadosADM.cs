using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa
{
    public class DireitosPorColaboradorAgrupadosADM
    {
        public virtual Evento Evento { get; set; }

        public Colaborador Colaborador { get; set; }
        public List<Direito> Direitos { get; set; }

    }
}