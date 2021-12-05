using System;
using System.Collections.Generic;

namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response
{
    public class BeneficiarioResponse
    {
        public int? Id { get; set; }

        public string Cpf { get; set; }

        public string NomeCompleto { get; set; }

        public DateTime? DataNascimento { get; set; }

        public int? UnidadeOrganizacionalId { get; set; }

        public string Edv { get; set; }

        public List<CriarEditarBeneficiarioDireitoResponseResumido> beneficios { get; set; }
    }
}