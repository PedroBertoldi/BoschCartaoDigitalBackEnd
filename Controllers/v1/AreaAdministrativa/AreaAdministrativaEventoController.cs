using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BoschCartaoDigitalBackEnd.Business.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Extentions;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
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
        public async Task<IActionResult> BuscarTodosEventos([FromQuery] PaginacaoRequest paginacao)
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
        public async Task<IActionResult> BuscarTodosEventos([FromRoute] int? id)
        {
            if (id == null) return BadRequest(new ErrorResponse("id é um campo obrigatório"));

            var evento = await _business.BuscarEventoPorIdAsync((int)id);
            return Ok(_mapper.Map<EventoResponse>(evento));
        }

        /// <summary>
        /// Cria um evento no banco de dados.
        /// </summary>
        /// <param name="request">Parametros necessário para criação de evento</param>
        [HttpPost]
        [ProducesResponseType(typeof(EventoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdicionarEvento([FromBody] CriarEditarEventoRequest request)
        {
            var evento = await _business.CriarEventoAsync(request);
            var erros = _business.BuscarErros();

            return (erros != null) ? BadRequest(erros) :
                Created(HttpContext.GetLocationURI($"api/AreaAdministrativa/Evento/{evento.Id}"), _mapper.Map<EventoResponse>(evento));
        }
        
        /// <summary>
        /// Modifica as informações de um evento.
        /// </summary>
        /// <param name="id">id do evento a ser modificado</param>
        /// <param name="request">Parametros necessários para a modificação</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EventoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarEvento([FromRoute] int? id, [FromBody] CriarEditarEventoRequest request)
        {
            if (id == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(id)));

            var evento = await _business.EditarEventoAsync(request, (int)id);
            var erros = _business.BuscarErros();

            return (erros == null) ? Ok(_mapper.Map<EventoResponse>(evento)) : BadRequest(erros);
        }
    }
}