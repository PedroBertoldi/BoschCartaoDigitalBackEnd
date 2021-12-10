using System;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response
{
    public class ColaboradorAlteradoResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Edv { get; set; }
        public virtual UnidadeOrganizacionalResponse UnidadeOrganizacional { get; set; }
    }
}