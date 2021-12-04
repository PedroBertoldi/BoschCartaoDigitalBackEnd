using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional
{
    public class DireitosAgrupadosAoColaborador
    {
        public Evento Evento { get; set; }
        public List<Direito> DireitosColaborador { get; set; }
        public Colaborador Colaborador { get; set; }
        public List<DireitosAgrupadosAoColaborador> SolicitadoParaRetirar { get; set; }
    }
}