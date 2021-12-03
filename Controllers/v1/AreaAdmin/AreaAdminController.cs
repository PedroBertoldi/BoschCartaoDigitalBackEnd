using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BoschCartaoDigitalBackEnd.Business.AreaAdmin;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaAdmin;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.AreaAdmin;

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

        /// <summary>
        /// Lista os beneficios de um evento.
        /// </summary>
        /// <param name="request">Parametros necessários para a consulta</param>
        [AllowAnonymous]
        [HttpGet("listar-beneficios-evento")]
        [ProducesResponseType(typeof(ListarBeneficiosEventoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarBeneficiosEvento([FromQuery] ListarBeneficiosEventoRequest request)
        {
            // var resposta = await _business.BuscarListaDireitosCompletaAsync(request);
            var resposta = await _business.ListaBeneficiosAsync(request);
            var erros = _business.BuscarErros();
            return (erros == null) ? Ok(_mapper.Map<ListarBeneficiosEventoResponse>(resposta)) : BadRequest(erros);
        }
    }
}