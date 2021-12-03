using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Repository.AreaAdmin;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;
using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaAdmin;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdmin;
// using BoschCartaoDigitalBackEnd.Exceptions.AreaPublica;

namespace BoschCartaoDigitalBackEnd.Business.AreaAdmin
{

    public class AreaAdminBusiness : BaseBussiness
    {
        private readonly AreaAdminRepository _repository;
        public AreaAdminBusiness(AreaAdminRepository repository) : base()
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista os beneficios de um evento.
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        /// <returns></returns>
        public async Task<ListaBeneficiosEvento> ListaBeneficiosAsync(ListarBeneficiosEventoRequest request)
        {
            ListaBeneficiosEvento resposta = default;
            return resposta;
        }
    }
}