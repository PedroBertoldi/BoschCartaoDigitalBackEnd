using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica
{
    public class DireitosPorColaboradorAgrupados
    {
        public Colaborador Colaborador { get; set; }
        public List<Direito> Direitos { get; set; }
        public virtual Evento Evento { get; set; }
        public List<DireitosPorColaboradorAgrupados> Indicacoes {get;set;}
    }
}