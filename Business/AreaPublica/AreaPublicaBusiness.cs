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

namespace BoschCartaoDigitalBackEnd.Business.AreaPublica
{

    public class AreaPublicaBusiness : BaseBussiness
    {
        private readonly AreaPublicaRepository _repository;
        public AreaPublicaBusiness(AreaPublicaRepository repository) : base()
        {
            _repository = repository;
        }

        public async Task<Evento> BuscarEventoAsync(int? eventoId)
        {
            var evento = (eventoId == null) ? await _repository.BuscarEventoAtivoAsync()
                : await _repository.BuscarEventoPorIdAsync((int)eventoId);
            return evento;
        }

        public async Task<DireitosPorColaboradorAgrupados> BuscarListaDireitosCompletaAsync(MeusBeneficiosRequest request)
        {
            DireitosPorColaboradorAgrupados resposta = default;
            var evento = await BuscarEventoAsync(request.EventoID);
            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.EventoID),
                    Message = (request.EventoID == null) ? "Atualmente não existe nem um evento ativo"
                        : $"Não foi encontrado nem um evento com o id: {request.EventoID}",
                });
                return null;
            }

            var colaborador = await _repository.BuscarColaboradorAsync(request.Cpf, request.DataNascimento.Value);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.Cpf),
                    Message = "Usuário não encontrado"
                });
                return null;
            }

            var direitos = await BuscarMeusDireitosAsync(evento.Id, colaborador.Id);
            var indicados = await BuscarDireitosIndicadosAsync(evento.Id, colaborador.Id);
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


            return resposta;
        }

        public async Task IndicarPessoaAsync(AdicionarIndicadoRequest request)
        {
            var evento = await BuscarEventoAsync(request.EventoId);
            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.EventoId),
                    Message = (request.EventoId == null) ? "Atualmente não existe nem um evento ativo"
                        : $"Não foi encontrado nem um evento com o id: {request.EventoId}",
                });
                return;
            }

            var indicado = (string.IsNullOrEmpty(request.Cpf)) ? await _repository.BuscarColaboradorPorEdvAsync(request.Edv) :
                await _repository.BuscarColaboradorPorCpfAsync(request.Cpf);
            
            if (indicado == null)
            {
                if (string.IsNullOrEmpty(request.NomeCompleto))
                {
                    _errors.Add(new ErrorModel{
                        FieldName = nameof(request.NomeCompleto),
                        Message = "Indicado não encontrado, na tentativa de cadastro o nome completo é necessário"
                    });
                    return;
                }
                indicado = await _repository.CadastrarNovoColaborador(request.Cpf, request.NomeCompleto);
            }
            var colaborador = await _repository.BuscarColaboradorPorIdAsync((int)request.ColaboradorId);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel{
                    FieldName = nameof(request.ColaboradorId),
                    Message = $"Não foi encontrado um colaborador com o ID: {request.ColaboradorId}",
                });
                return;
            }

            if (colaborador.Id == indicado.Id) return;

            await _repository.CadastrarIndicadoEmDireitosAsync(colaborador.Id,evento.Id,indicado.Id,request.DireitosId);
        }

        public async Task RemoverIndicacoesAsync(RemoverIndicadoRequest request)
        {
            var evento = await BuscarEventoAsync(request.EventoId);
            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.EventoId),
                    Message = (request.EventoId == null) ? "Atualmente não existe nem um evento ativo"
                        : $"Não foi encontrado nem um evento com o id: {request.EventoId}",
                });
                return;
            }
            var colaborador = await _repository.BuscarColaboradorPorIdAsync((int)request.ColaboradorId);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel{
                    FieldName = nameof(request.ColaboradorId),
                    Message = $"Não foi encontrado um colaborador com o ID: {request.ColaboradorId}",
                });
                return;
            }
            await _repository.RemoverIndicacoesEmDireitosAsync(colaborador.Id, evento.Id, request.DireitosId);
        }

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
        public async Task<List<Direito>> BuscarDireitosIndicadosAsync(int eventoId, int userId)
        {
            return await _repository.BuscarDireitosIndicadosAsync(eventoId, userId);
        }
    }
}