using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BoschCartaoDigitalBackEnd.Business.AreaAdmin;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;
// using BoschCartaoDigitalBackEnd.Models.v1.Responses.AreaPublica;
// using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.AreaAdmin
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AreaAdminController : ControllerBase
    {
        private readonly AreaAdminBusiness _business;
        private readonly IMapper _mapper;
        public AreaAdminController(AreaAdminBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }
    }
}