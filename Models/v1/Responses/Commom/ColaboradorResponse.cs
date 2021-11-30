using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom
{
    public class ColaboradorResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public virtual UnidadeOrganizacionalResponse UnidadeOrganizacional { get; set; }
    }
}