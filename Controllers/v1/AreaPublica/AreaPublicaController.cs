using Microsoft.AspNetCore.Mvc;
using BoschCartaoDigitalBackEnd.Business.AreaPublica;
using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica;
using System.Threading.Tasks;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using Microsoft.AspNetCore.Http;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.AreaPublica;
using System.Linq;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.AreaPublica
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AreaPublicaController : ControllerBase
    {
        private readonly AreaPublicaBusiness _business;
        private readonly IMapper _mapper;
        public AreaPublicaController(AreaPublicaBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca os direitos relacionados a um colaborador, seus direitos como direitos indicados.
        /// </summary>
        /// <param name="request">Parametros necessários para a busca</param>
        [AllowAnonymous]
        [HttpGet("buscar-direitos")]
        [ProducesResponseType(typeof(DireitosPorColaboradorAgrupadosResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscarDireitos([FromQuery] MeusBeneficiosRequest request)
        {
            var resposta = await _business.BuscarListaDireitosCompletaAsync(request);
            var erros = _business.BuscarErros();
            return (erros == null) ? Ok(_mapper.Map<DireitosPorColaboradorAgrupadosResponse>(resposta)) : BadRequest(erros);
        }

        
    }
}