using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Exceptions.Commom;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaOperacional;

namespace BoschCartaoDigitalBackEnd.Business.AreaOperacional
{
    public class AreaOperacionalBusiness : BaseBusiness
    {
        private readonly AreaOperacionalRepository _repository;

        public AreaOperacionalBusiness(AreaOperacionalRepository repository)
        {
            _repository = repository;
        }

        private async Task<Evento> BuscarProximoEventoOuEvetoPorIdAsync(int? id)
        {
            var evento = (id == null) ? await _repository.BuscarProximoEventoAsync() : await _repository.BuscarEventoPorIdAsync((int)id);
            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = "EventoId",
                    Message = (id == null) ? "Não foi encontrado nem um evento ativo ou proximo" : $"Não foi encontrado nem um evento com o ID: {id}"
                });
                throw new OperacaoInvalidaException();
            }
            return evento;
        }

        public async Task<DireitosAgrupadosAoColaborador> BuscarBeneficiosPorCPFOuEDV(BuscarDireitosRequest request)
        {
            DireitosAgrupadosAoColaborador resposta = default;

            try
            {
                var colaborador = (string.IsNullOrEmpty(request.Cpf)) ? await _repository.BuscarColaboradorPorEdvAsync(request.Edv.Trim()) :
                await _repository.BuscarColaboradorPorCPFAsync(request.Cpf.Trim());

                if (colaborador == null)
                {
                    var cpfNulo = string.IsNullOrEmpty(request.Cpf);
                    _errors.Add(new ErrorModel
                    {
                        FieldName = (cpfNulo) ? nameof(request.Edv) : nameof(request.Cpf),
                        Message = $"Não foi possível encontrar colaborador com o {(cpfNulo ? "EDV" : "CPF")} : {(cpfNulo ? request.Edv : request.Cpf)}"
                    });
                    return null;
                }

                var evento = await BuscarProximoEventoOuEvetoPorIdAsync(request.EventoId);

                var beneficio = await _repository.BuscarDireitosPorColaboradorIdAsync(colaborador.Id, evento.Id);
                var solicitadoParaRetirar = await _repository.BuscarDireitosIndicadosPorColaboradorIdAsync(colaborador.Id, evento.Id);

                resposta = new DireitosAgrupadosAoColaborador
                {
                    Evento = evento,
                    Colaborador = colaborador,
                    DireitosColaborador = beneficio,
                    SolicitadoParaRetirar = (solicitadoParaRetirar.Count > 0) ? solicitadoParaRetirar.GroupBy(s => s.ColaboradorId).Select(g => new DireitosAgrupadosAoColaborador
                    {
                        Evento = evento,
                        Colaborador = g.First().Colaborador,
                        DireitosColaborador = g.ToList(),
                        SolicitadoParaRetirar = null,
                    }).ToList() : new List<DireitosAgrupadosAoColaborador>(),
                };
            }
            catch (OperacaoInvalidaException) { }

            return resposta;
        }

        public async Task<List<Direito>> CadastrarRecebimentoAsync(DireitoEntregueRequest request)
        {
            List<Direito> retorno = default;
            try
            {
                Colaborador colaborador = default;
                if (!string.IsNullOrEmpty(request.CpfRecebedor))
                {
                    colaborador = await _repository.BuscarColaboradorPorCPFAsync(request.CpfRecebedor);
                    if (colaborador == null)
                    {
                        _errors.Add(new ErrorModel
                        {
                            FieldName = nameof(request.CpfRecebedor),
                            Message = $"Indicado com o CPF: {request.CpfRecebedor} não encontrado"
                        });
                        return null;
                    }
                }else{
                    colaborador = await _repository.BuscarColaboradorPorIDAsync((int)request.ColaboradorId);
                    if (colaborador == null)
                    {
                        _errors.Add(new ErrorModel
                        {
                            FieldName = nameof(request.ColaboradorId),
                            Message = $"Colaborador com o ID: {request.ColaboradorId} não encontrado"
                        });
                        return null;
                    }
                }
                var evento = await BuscarProximoEventoOuEvetoPorIdAsync(request.EventoId);
                retorno = await _repository.DefinirDireitoComoRecebidoAsync(request.DireitosEntregues,evento.Id, colaborador.Id);
            }
            catch (OperacaoInvalidaException){}
            return retorno;
        }
    }
}