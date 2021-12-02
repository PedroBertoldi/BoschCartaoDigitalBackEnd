using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response
{
    public class ColaboradorResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Edv { get; set; }
        public virtual UnidadeOrganizacionalResponse UnidadeOrganizacional { get; set; }
    }
}