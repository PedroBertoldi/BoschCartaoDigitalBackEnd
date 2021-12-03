using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BoschCartaoDigitalBackEnd.Business.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.AreaAdministrativa
{
    [ApiController]
    [Route("api/AreaAdministrativa/Evento")]
    [Produces("application/json")]
    public class AreaAdministrativaEventoController : ControllerBase
    {
        private readonly AreaAdministrativaBusiness _business;
        private readonly IMapper _mapper;

        public AreaAdministrativaEventoController(AreaAdministrativaBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }
        /// <summary>
        /// Retorna todos os eventos cadastrados, suporta paginação.
        /// </summary>
        /// <param name="paginacao">parametros para paginação</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<EventoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarTodosEventos([FromQuery]PaginacaoRequest paginacao)
        {
            var eventos = await _business.BuscarTodosEventosAsync(paginacao);
            return Ok(_mapper.Map<List<EventoResponse>>(eventos));
        }
        /// <summary>
        /// Retorna um evento em especifico.
        /// </summary>
        /// <param name="id">Id do evento</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarTodosEventos([FromRoute]int? id)
        {
            if(id == null) return BadRequest(new ErrorResponse("id é um campo obrigatório"));
            
            var evento = await _business.BuscarEventoPorIdAsync((int)id);
            return Ok(_mapper.Map<EventoResponse>(evento));
        }

        /// <summary>
        /// Lista os beneficios de um evento.
        /// </summary>
        /// <param name="request">Parametros necessários para a consulta</param>
        // [AllowAnonymous]
        [HttpGet("listar-beneficios")]
        [ProducesResponseType(typeof(List<ListarBeneficiosEventoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarBeneficiosEvento([FromQuery] ListarBeneficiosEventoRequest request)
        {
            var resposta = await _business.ListaBeneficiosAsync(request);
            var erros = _business.BuscarErros();
            return (erros == null) ? Ok(_mapper.Map<List<ListarBeneficiosEventoResponse>>(resposta)) : BadRequest(erros);
        }
    }
}