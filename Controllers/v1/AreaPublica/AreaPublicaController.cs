using Microsoft.AspNetCore.Mvc;
using BoschCartaoDigitalBackEnd.Business.AreaPublica;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Linq;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Request;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;

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
        
        /// <summary>
        /// Indica uma pessoa para receber seus direitos, se a pessoa não existir no banco de dados é cadastrada.
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        [AllowAnonymous] //Verificar isso aqui, não sei se é a melhor coisa
        [HttpPut("indicar-pessoa")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IndicarPessoa([FromBody] AdicionarIndicadoRequest request)
        {
            if (string.IsNullOrEmpty(request.Cpf) && string.IsNullOrEmpty(request.Edv))
                return BadRequest(new ErrorResponse($"Um dos campos {nameof(request.Cpf)} ou {nameof(request.Edv)} deve estar preenchido"));
            await _business.IndicarPessoaAsync(request);
            var erros = _business.BuscarErros();

            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Remove as indicações de retirada de direitos.
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        [AllowAnonymous]
        [HttpPut("remover-indicacoes")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoverIndicacoes([FromBody]RemoverIndicadoRequest request)
        {
            await _business.RemoverIndicacoesAsync(request);
            var erros = _business.BuscarErros();
            return (erros == null) ? NoContent() : BadRequest(erros);
        }

        /// <summary>
        /// Busca um colaborador por edv.
        /// </summary>
        /// <param name="edv">EDV do colaborador</param>
        [AllowAnonymous]
        [HttpGet("buscar-colaborador/{edv}")]
        public async Task<IActionResult> BuscarColaboradorPorEdv([FromRoute] string edv)
        {
            if(string.IsNullOrEmpty(edv)) return BadRequest(new ErrorResponse($"{nameof(edv)} é um campo obrigatório"));

            var colaborador = await _business.BuscarColaboradorPorEdv(edv);

            return (colaborador == null) ? NotFound(new ErrorResponse("Colaborador não encontrado")) : Ok(_mapper.Map<ColaboradorResponse>(colaborador));
        }
    }
}