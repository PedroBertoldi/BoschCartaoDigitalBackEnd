using System;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;


namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response
{
    public class ColaboradorAlterado
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Edv { get; set; }
        public virtual UnidadeOrganizacional UnidadeOrganizacional { get; set; }
    }
}