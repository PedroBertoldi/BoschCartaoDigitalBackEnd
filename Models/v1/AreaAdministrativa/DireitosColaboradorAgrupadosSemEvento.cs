using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa
{
    public class DireitosColaboradorAgrupadosSemEvento
    {
        public ColaboradorAlterado Colaborador { get; set; }

        public string origem { get; set; }
        public Colaborador Indicado { get; set; }
        public List<Direito> Direitos { get; set; }

    }
}