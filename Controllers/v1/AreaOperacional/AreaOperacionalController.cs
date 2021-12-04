using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BoschCartaoDigitalBackEnd.Business.AreaOperacional;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Request;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Response;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.AreaOperacional
{
    [Authorize(Roles = "Entrega")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AreaOperacionalController : ControllerBase
    {
        private readonly AreaOperacionalBusiness _business;
        private readonly IMapper _mapper;
        public AreaOperacionalController(AreaOperacionalBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os direitos relacionados a um cpf ou edv.
        /// </summary>
        /// <param name="request"></param>
        [HttpGet("Direitos")]
        [ProducesResponseType(typeof(DireitosAgrupadosAoColaboradorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuscarDireitos([FromQuery] BuscarDireitosRequest request)
        {
            if (string.IsNullOrEmpty(request.Cpf) && string.IsNullOrEmpty(request.Edv)) 
                return BadRequest(new ErrorResponse("É necessário ao menos o CPF ou EDV do colaborador"));

            var resposta = await _business.BuscarBeneficiosPorCPFOuEDV(request);
            var erros = _business.BuscarErros();

            return (erros == null) ? Ok(_mapper.Map<DireitosAgrupadosAoColaboradorResponse>(resposta)) : BadRequest(erros);
        }

        /// <summary>
        /// Define uma lista de direitos como recebidos.
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        /// <returns></returns>
        [HttpPost("Direitos/receber")]
        [ProducesResponseType(typeof(List<DireitoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReceberDireitos([FromBody] DireitoEntregueRequest request)
        {
            if (!string.IsNullOrEmpty(request.CpfRecebedor) && request.ColaboradorId == null)
                return BadRequest(new ErrorResponse("ID do colaborador ou CPF do terceito é necessário"));

            var direitos = await _business.CadastrarRecebimentoAsync(request);
            var erros = _business.BuscarErros();
            return (erros == null) ? Ok(_mapper.Map<List<DireitoResponse>>(direitos)) : BadRequest(erros);
        }
    }
}