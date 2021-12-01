using BoschCartaoDigitalBackEnd.Database.Context;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaPublica;
using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaPublica;
using Microsoft.AspNetCore.Mvc;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica;
using BoschCartaoDigitalBackEnd.Exceptions.AreaPublica;

namespace BoschCartaoDigitalBackEnd.Business.AreaPublica
{

    public class AreaPublicaBusiness : BaseBussiness
    {
        private readonly AreaPublicaRepository _repository;
        public AreaPublicaBusiness(AreaPublicaRepository repository) : base()
        {
            _repository = repository;
        }

        /// <summary>
        /// Busca o primeiro evento ativou ou o evento com o id informado.
        /// </summary>
        /// <param name="eventoId">id do evento a ser procurado ou nulo para o primeiro ativo</param>
        private async Task<Evento> BuscarEventoAsync(int? eventoId)
        {
            var evento = (eventoId == null) ? await _repository.BuscarEventoAtivoAsync()
                : await _repository.BuscarEventoPorIdAsync((int)eventoId);

            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(eventoId),
                    Message = (eventoId == null) ? "Atualmente não existe nem um evento ativo"
                        : $"Não foi encontrado nem um evento com o id: {eventoId}",
                });
                throw new OperacaoInvalidaException();
            }
            return evento;
        }

        /// <summary>
        /// Busca um colaborador por CPF e data de nascimento.
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        private async Task<Colaborador> BuscarColaboradorAsync(string cpf, DateTime dataNascimento)
        {
            var colaborador = await _repository.BuscarColaboradorAsync(cpf, dataNascimento);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(cpf),
                    Message = "Usuário não encontrado"
                });
                throw new OperacaoInvalidaException();
            }
            return colaborador;
        }

        /// <summary>
        /// Busca um colaborador por seu ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Colaborador> BuscarColaboradorPorIdAsync(int id)
        {
            var colaborador = await _repository.BuscarColaboradorPorIdAsync(id);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"Não foi encontrado um colaborador com o ID: {id}",
                });
                throw new OperacaoInvalidaException();
            }
            return colaborador;
        }

        /// <summary>
        /// Busca todos os direitos de um colaborador e os direitos de outros colaboradores que o indicaram para retirada.
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        /// <returns></returns>
        public async Task<DireitosPorColaboradorAgrupados> BuscarListaDireitosCompletaAsync(MeusBeneficiosRequest request)
        {
            DireitosPorColaboradorAgrupados resposta = default;
            try
            {
                var evento = await BuscarEventoAsync(request.EventoID);
                var colaborador = await BuscarColaboradorAsync(request.Cpf, request.DataNascimento.Value);
                var direitos = await BuscarMeusDireitosAsync(evento.Id, colaborador.Id);
                var indicados = await _repository.BuscarDireitosIndicadosAsync(evento.Id, colaborador.Id);
                if (direitos.Count > 0 || indicados.Count > 0)
                {
                    resposta = new DireitosPorColaboradorAgrupados
                    {
                        Colaborador = colaborador,
                        Evento = evento,
                        Direitos = direitos,
                        Indicacoes = indicados.GroupBy(i => i.ColaboradorId).ToList().Select(g => new DireitosPorColaboradorAgrupados
                        {
                            Colaborador = g.First().Colaborador,
                            Evento = evento,
                            Direitos = g.ToList(),
                        }).ToList(),
                    };
                }
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }

            return resposta;
        }

        /// <summary>
        /// Indica uma pessoa para retirada de direitos.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task IndicarPessoaAsync(AdicionarIndicadoRequest request)
        {
            try
            {
                var evento = await BuscarEventoAsync(request.EventoId);
                var indicado = (string.IsNullOrEmpty(request.Cpf)) ? await _repository.BuscarColaboradorPorEdvAsync(request.Edv) :
                    await _repository.BuscarColaboradorPorCpfAsync(request.Cpf);

                if (indicado == null)
                {
                    if (string.IsNullOrEmpty(request.NomeCompleto))
                    {
                        _errors.Add(new ErrorModel
                        {
                            FieldName = nameof(request.NomeCompleto),
                            Message = "Indicado não encontrado, na tentativa de cadastro o nome completo é necessário"
                        });
                        return;
                    }
                    indicado = await _repository.CadastrarNovoColaborador(request.Cpf, request.NomeCompleto);
                }
                var colaborador = await BuscarColaboradorPorIdAsync((int)request.ColaboradorId);

                if (colaborador.Id == indicado.Id) return;

                await _repository.CadastrarIndicadoEmDireitosAsync(colaborador.Id, evento.Id, indicado.Id, request.DireitosId);
            }
            catch (OperacaoInvalidaException)
            {
                return;
            }
        }

        /// <summary>
        /// Remove as indicações de retirada de direitos.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task RemoverIndicacoesAsync(RemoverIndicadoRequest request)
        {
            try
            {
                var evento = await BuscarEventoAsync(request.EventoId);
                var colaborador = await BuscarColaboradorPorIdAsync((int)request.ColaboradorId);
                await _repository.RemoverIndicacoesEmDireitosAsync(colaborador.Id, evento.Id, request.DireitosId);
            }
            catch (OperacaoInvalidaException)
            {
                return;
            }
        }

        /// <summary>
        /// Busca um colaborador por seu EDV
        /// </summary>
        /// <param name="edv"></param>
        /// <returns></returns>
        public async Task<Colaborador> BuscarColaboradorPorEdv(string edv)
        {
            return await _repository.BuscarColaboradorPorEdvAsync(edv);
        }

        /// <summary>
        /// Faz uma busca nos direitos e retorna os direitos disponíveis para o usuário.
        /// </summary>
        public async Task<List<Direito>> BuscarMeusDireitosAsync(int eventoId, int userId)
        {
            return await _repository.BuscarDireitosAsync(eventoId, userId);
        }
    }
}