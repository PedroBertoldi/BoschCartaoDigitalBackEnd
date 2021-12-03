using System;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.AreaAdmin
{
    public class ListarBeneficiosEventoResponse
    {
        public List<BeneficioResponse> Beneficios { get; set; }

    }
}