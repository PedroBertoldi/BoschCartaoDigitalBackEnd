using System.Collections.Generic;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using System;
using BoschCartaoDigitalBackEnd.Exceptions.AreaPublica;

namespace BoschCartaoDigitalBackEnd.Business.AreaAdministrativa
{
    public class AreaAdministrativaBusiness : BaseBusiness
    {
        private readonly AreaAdministrativaRepository _repository;

        public AreaAdministrativaBusiness(AreaAdministrativaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Evento>> BuscarTodosEventosAsync(PaginacaoRequest paginacao)
        {
            return await _repository.BuscarTodosEventosAsync(paginacao);
        }

        public async Task<Evento> BuscarEventoPorIdAsync(int id)
        {
            return await _repository.BuscarEventoPorIdAsync(id);
        }

        public async Task<Evento> CriarEventoAsync(CriarEditarEventoRequest request)
        {
            Evento retono = default;
            try
            {
                ValidarDataEvento((DateTime)request.Inicio, (DateTime)request.Fim);
                retono = new Evento
                {
                    Nome = request.NomeEvento.Trim(),
                    Descricao = request.Descricao,
                    DataInicio = request.Inicio,
                    DataFim = request.Fim
                };
                await _repository.AdicionarEventoAsync(retono);
            }
            catch (OperacaoInvalidaException)
            {
            }

            return retono;
        }

        private void ValidarDataEvento(DateTime inicio, DateTime fim)
        {
            if (fim.Date <= inicio.Date)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(inicio),
                    Message = "A data de inicio deve se maior que a data de fim do evento",
                });
                throw new OperacaoInvalidaException();
            }
        }

        public async Task<Evento> EditarEventoAsync(CriarEditarEventoRequest request, int id)
        {
            var evento = await _repository.BuscarEventoPorIdAsync(id);
            try
            {
                if (evento == null)
                {
                    _errors.Add(new ErrorModel
                    {
                        FieldName = nameof(id),
                        Message = $"Não foi possível encontrar um evento com o ID: {id}",
                    });
                    return null;
                }

                ValidarDataEvento((DateTime)request.Inicio, (DateTime)request.Fim);

                evento.Nome = request.NomeEvento.Trim();
                evento.Descricao = request.Descricao;
                evento.DataInicio = request.Inicio;
                evento.DataFim = request.Fim;
                await _repository.EditarEventoAsync(evento);
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }

            return evento;
        }
    }
}