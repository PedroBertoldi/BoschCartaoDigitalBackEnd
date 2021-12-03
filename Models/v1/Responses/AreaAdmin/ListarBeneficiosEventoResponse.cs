using System;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.AreaAdmin
{
    public class ListarBeneficiosEventoResponse
    {
        public List<BeneficioResponse> Beneficios { get; set; }

    }
}