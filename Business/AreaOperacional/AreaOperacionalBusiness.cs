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
using System;

namespace BoschCartaoDigitalBackEnd.Business.AreaOperacional
{
    public class AreaOperacionalBusiness : BaseBusiness
    {
        private readonly AreaOperacionalRepository _repository;

        public AreaOperacionalBusiness(AreaOperacionalRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Função interna auxiliar para buscar um evento por ID ou o proximo evento que ira ocorrer.
        /// </summary>
        /// <param name="id">Id do evento, null para o proximo evento</param>
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
        /// <summary>
        /// Busca beneficios de um colaborador pelo CPF ou EDV, sem a necessidade da data de nascimento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Define um grupo de direitos como recebidos
        /// </summary>
        /// <param name="request">Parametros necessarios</param>
        /// <returns></returns>
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
    

        /// <summary>
        /// Define um grupo de direitos como recebidos
        /// </summary>
        /// <param name="idEvento">Parametros necessarios</param>
        /// <returns></returns>
        public async Task<List<ColaboradoresAgrupadosAoDireito>> ListarColaboradoresPorBeneficio(int idEvento)
        {
            List<ColaboradoresAgrupadosAoDireito> resposta = new List<ColaboradoresAgrupadosAoDireito>();

            try{
                var evento = await BuscarProximoEventoOuEvetoPorIdAsync(idEvento);
                var beneficiosNoEvento = await _repository.BuscarTodosBeneficiosEmEvento(idEvento);
                if(beneficiosNoEvento==null)                    
                {
                        _errors.Add(new ErrorModel
                        {
                            FieldName = nameof(idEvento),
                            Message = $"Não há benefícios cadastrados no evento: {idEvento}"
                        });
                        return null;
                }
                var colaboradores= await _repository.BuscarTodosColaboradoresBosch();
                if(beneficiosNoEvento==null)                    
                {
                        _errors.Add(new ErrorModel
                        {
                            FieldName = nameof(colaboradores),
                            Message = $"Não há colaboradores cadastrados: {colaboradores}"
                        });
                        return null;
                }
                foreach(Beneficio b in beneficiosNoEvento){
                    var pacote = new ColaboradoresAgrupadosAoDireito();
                    pacote.beneficiarios = new List<BeneficiarioResumido>();
                    pacote.beneficio=b;
                    foreach(Colaborador c in colaboradores){
                        var direitos = await _repository.BuscarBeneficiosEspecificosEmEventoPorIdColaborador(c.Id, idEvento, b.Id);
                        if(direitos==null){continue;}
                        foreach(Direito d in direitos){
                            var agrupado = new BeneficiarioResumido();
                            agrupado.idColaborador=c.Id;
                            agrupado.nomeCompleto = c.NomeCompleto;
                            agrupado.idDireito=d.Id;
                            pacote.beneficiarios.Add(agrupado);
                        }

                    }
                    resposta.Add(pacote);
                }                
            }
            catch (OperacaoInvalidaException) { }
            return resposta;

        }
    }
}