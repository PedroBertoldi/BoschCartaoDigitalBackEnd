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
// using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica;
// using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica;
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

    }
}