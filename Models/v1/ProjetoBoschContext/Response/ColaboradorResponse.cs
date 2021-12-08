using System;

namespace BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response
{
    public class ColaboradorResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Edv { get; set; }
        public virtual UnidadeOrganizacionalResponse UnidadeOrganizacional { get; set; }
        public int? OrigemId { get; set; }
        public DateTime? DataDeCadastro { get; set; }
    }
}