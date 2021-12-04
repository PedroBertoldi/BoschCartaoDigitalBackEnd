using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BoschCartaoDigitalBackEnd.Business.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Extentions;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.AreaAdministrativa
{
    [ApiController]
    [Route("api/AreaAdministrativa")]
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
        [HttpGet("Evento")]
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
        [HttpGet("Evento/{id}")]
        [ProducesResponseType(typeof(EventoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarTodosEventos([FromRoute] int? id)
        {
            if (id == null) return BadRequest(new ErrorResponse("id é um campo obrigatório"));

            var evento = await _business.BuscarEventoPorIdAsync((int)id);
            return Ok(_mapper.Map<EventoResponse>(evento));
        }

        /// <summary>
        ///  Cria um evento no banco de dados.
        /// </summary>
        /// <param name="request">Parametros necessário para criação de evento</param>
        [HttpPost("Evento")]
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
        [HttpPut("Evento/{id}")]
        [ProducesResponseType(typeof(EventoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarEvento([FromRoute] int? id, [FromBody] CriarEditarEventoRequest request)
        {
            if (id == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(id)));

            var evento = await _business.EditarEventoAsync(request, (int)id);
            var erros = _business.BuscarErros();

            return (erros == null) ? Ok(_mapper.Map<EventoResponse>(evento)) : BadRequest(erros);
        }

        /// <summary>
        /// Exclui um Evento pelo Id em cascata
        /// </summary>
        /// <param name="id">beneficioId do Direito a ser excluido</param>
        [HttpDelete("Evento/{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirEventoId([FromRoute] int? id)
        {
            if (id == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(id)));

            // await _business.ExcluirEventoIdAsync((int)id);
            await _business.ExcluirEventoIdCascataAsync((int)id);
            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Lista os beneficios de um evento.
        /// </summary>
        /// <param name="id">Parametros necessários para a consulta</param>
        // [AllowAnonymous]
        [HttpGet("Evento/{id}/listar-beneficios")]
        [ProducesResponseType(typeof(List<ListarBeneficiosEventoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListarBeneficiosEvento([FromRoute] int? id)
        {
            if(id == null) return BadRequest(new ErrorModel("ID é um parametro necessário", nameof(id)));

            var resposta = await _business.ListaBeneficiosPorEventoAsync((int)id);
            var erros = _business.BuscarErros();
            return (erros == null) ? Ok(_mapper.Map<List<ListarBeneficiosEventoResponse>>(resposta)) : BadRequest(erros);
        }

        /// <summary>
        /// Cadastra um tipo de beneficio no banco de dados.
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        [HttpPost("Beneficio")]
        [ProducesResponseType(typeof(BeneficioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarBeneficio([FromBody] CriarEditarBeneficioRequest request)
        {
            var beneficio = await _business.CadastrarBeneficioAsync(request);
            var erros = _business.BuscarErros();

            return(erros == null) ? Created(HttpContext.GetLocationURI($"api/beneficio/{beneficio.Id}"), _mapper.Map<BeneficioResponse>(beneficio)) : 
                BadRequest(erros); 
        }
        
        /// <summary>
        /// Cria uma relação entre um beneficio e um evento.
        /// </summary>
        /// <param name="request">Parametros necessarios para criar a relação</param>
        [HttpPost("Beneficio/criar-relacao")]
        [ProducesResponseType(typeof(BeneficioEventoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarRelacaoBeneficioEvento([FromBody] RelacaoBeneficioEventoRequest request)
        {
            var relacao = await _business.CriarRelacaoBeneficioEventoAsync(request);
            var erros = _business.BuscarErros();
            return (erros != null) ? BadRequest(erros) : Ok(_mapper.Map<BeneficioEventoResponse>(relacao));
        }

        /// <summary>
        /// Modifica as informações de um Beneficio.
        /// </summary>
        /// <param name="id">id do beneficio a ser modificado</param>
        /// <param name="request">Parametros necessários para a modificação</param>
        [HttpPut("Beneficio/{id}")]
        [ProducesResponseType(typeof(BeneficioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarBeneficio([FromRoute] int? id, [FromBody] CriarEditarBeneficioRequest request)
        {
            if (id == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(id)));

            var beneficio = await _business.EditarBeneficioAsync((int)id, request);
            var erros = _business.BuscarErros();

            return (erros == null) ? Ok(_mapper.Map<BeneficioResponse>(beneficio)) : BadRequest(erros);
        }

        /// <summary>
        /// Exclui um Beneficio pelo id em cascata.
        /// </summary>
        /// <param name="id">id do beneficio a ser excluido</param>
        [HttpDelete("Beneficio/{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirBeneficio([FromRoute] int? id)
        {
            if (id == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(id)));

            // await _business.ExcluirBeneficioAsync((int)id);
            await _business.ExcluirBeneficioCascataAsync((int)id);
            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Exclui BeneficioEvento pelo beneficioId.
        /// </summary>
        /// <param name="beneficioId">beneficioId do BeneficioEvento a ser excluido</param>
        [HttpDelete("BeneficioEvento/Beneficio/{beneficioId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirBeneficioEventoIdBeneficio([FromRoute] int? beneficioId)
        {
            if (beneficioId == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(beneficioId)));

            await _business.ExcluirBeneficioEventoIdBeneficioAsync((int)beneficioId);

            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Exclui BeneficioEvento pelo eventoId.
        /// </summary>
        /// <param name="eventoId">eventoId do BeneficioEvento a ser excluido</param>
        [HttpDelete("BeneficioEvento/Evento/{eventoId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirBeneficioEventoIdEvento([FromRoute] int? eventoId)
        {
            if (eventoId == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(eventoId)));

            await _business.ExcluirBeneficioEventoIdEventoAsync((int)eventoId);

            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Exclui Direito pelo beneficioId.
        /// </summary>
        /// <param name="beneficioId">beneficioId do Direito a ser excluido</param>
        [HttpDelete("Direito/Beneficio/{beneficioId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirDireitoIdBeneficio([FromRoute] int? beneficioId)
        {
            if (beneficioId == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(beneficioId)));

            await _business.ExcluirDireitoIdBeneficioAsync((int)beneficioId);

            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Exclui Direito pelo eventoId.
        /// </summary>
        /// <param name="eventoId">eventoId do Direito a ser excluido</param>
        [HttpDelete("Direito/Evento/{eventoId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirDireitoIdEvento([FromRoute] int? eventoId)
        {
            if (eventoId == null) return BadRequest(new ErrorResponse("O campo ID é obrigatório", nameof(eventoId)));

            await _business.ExcluirDireitoIdEventoAsync((int)eventoId);

            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Retorna todos os beneficios cadastrados independente de evento
        /// </summary>
        [HttpGet("Beneficio")]
        [ProducesResponseType(typeof(List<BeneficioResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarTodosBeneficios()
        {
            var beneficios = await _business.BuscarTodosBeneficiosAsync();
            var erros = _business.BuscarErros();
            return (erros == null) ? Ok(_mapper.Map<List<BeneficioResponse>>(beneficios)) : BadRequest(erros);
        }

        /// <summary>
        /// Retorna um beneficio em especifico
        /// </summary>
        /// <param name="id">Id do beneficio</param>
        [HttpGet("Beneficio/{id}")]
        [ProducesResponseType(typeof(BeneficioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscarBeneficioPorID([FromRoute] int? id)
        {
            if (id == null) return BadRequest(new ErrorResponse("ID é um campo necessário", nameof(id)));

            var beneficio = await _business.BuscarUnicoBeneficioPorIdAsync((int)id);
            var erros = _business.BuscarErros();

            return (erros == null) ? Ok(_mapper.Map<BeneficioResponse>(beneficio)) : BadRequest(erros);
        }

        [HttpPost("Evento/criar-beneficio-no-evento")]
        [ProducesResponseType(typeof(BeneficioEventoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarBeneficioEAtrelar([FromBody] CriarEAtrelarBeneficioRequest request)
        {
            var beneficioEvento = await _business.CriarEAtrelarBeneficioAsync(request);
            var erros = _business.BuscarErros();

            return (erros == null) ? Ok(_mapper.Map<BeneficioEventoResponse>(beneficioEvento)) : BadRequest(erros);
        }
    }
}