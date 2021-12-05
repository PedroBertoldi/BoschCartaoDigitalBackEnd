using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa
{
    public class DireitosColaboradorAgrupadosSemEvento
    {
        public Colaborador Colaborador { get; set; }
        public List<Direito> Direitos { get; set; }

    }
}