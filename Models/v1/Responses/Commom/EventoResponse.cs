using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom
{
    public class EventoResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}