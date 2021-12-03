using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response
{
    public class EventoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}