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

        public async Task<Colaborador> BuscarColaboradorAsync(string cpf, DateTime dataNascimento)
        {
            var colaborador = await _repository.BuscarColaboradorAsync(cpf, dataNascimento.Date);
            return colaborador;
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

            var colaborador = await BuscarColaboradorAsync(request.Cpf, request.DataNascimento.Value);
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