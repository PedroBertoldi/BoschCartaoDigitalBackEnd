using System.Collections.Generic;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Business.Commom;
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

        public async Task<List<Beneficio>> ListaBeneficiosAsync(int id)
        {
            List<Beneficio> lista = await _repository.ListaBeneficioIdEventoAsync((int)id);
            if (lista == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"Nenhum beneficio encontrado com este eventoId: {(int)id}",
                });
                return null;
            }
            return lista;
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

        public async Task<Beneficio> CadastrarBeneficioAsync(CriarEditarBeneficioRequest request)
        {
            var teste = await _repository.BuscarBeneficioPorDescricaoAsync(request.Beneficio);
            if (teste != null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.Beneficio),
                    Message = $"Já existe um beneficio com a seguinte descrição: {request.Beneficio}"
                });
                return null;
            }

            var temp = new Beneficio
            {
                Descricao = request.Beneficio.Trim(),
            };

            await _repository.CriarBeneficioAsync(temp);
            return temp;
        }

        public async Task<BeneficioEvento> CriarRelacaoBeneficioEventoAsync(RelacaoBeneficioEventoRequest request)
        {
            var testeBeneficio = await _repository.BuscarBeneficioPorIdAsync((int)request.BeneficioId);
            if (testeBeneficio == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.BeneficioId),
                    Message = $"O beneficio com o ID : {request.BeneficioId} não foi encontrado",
                });
                return null;
            }

            var testeEvento = await _repository.BuscarEventoPorIdAsync((int)request.EventoId);
            if (testeEvento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.EventoId),
                    Message = $"Não foi encontrado um evento com o id: {request.EventoId}",
                });
                return null;
            }
            var temp = new BeneficioEvento
            {
                BeneficioId = testeBeneficio.Id,
                EventoId = testeEvento.Id,
            };
            await _repository.CriarRelacaoBeneficioEventoAsync(temp);
            return temp;
        }

        private async Task<Beneficio> ValidarBeneficioPorDescricaoAsync(string descricao)
        {
            var beneficio = await _repository.BuscarBeneficioPorDescricaoAsync(descricao);
            if (beneficio != null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(descricao),
                    Message = $"Já existe um beneficio com a seguinte descrição: {descricao}"
                });
                throw new OperacaoInvalidaException();
            }
            return beneficio;
        }

        private async Task<Beneficio> BuscarBeneficioPorIdAsync(int id)
        {
            var beneficio = await _repository.BuscarBeneficioPorIdAsync(id);
            if (beneficio == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"O beneficio com o ID : {id} não foi encontrado",
                });
                throw new OperacaoInvalidaException();
            }
            return beneficio;
        }

        public async Task<Beneficio> EditarBeneficioAsync(int id, CriarEditarBeneficioRequest request)
        {
            Beneficio beneficio = default;
            try
            {
                beneficio = await BuscarBeneficioPorIdAsync(id);

                await ValidarBeneficioPorDescricaoAsync(request.Beneficio.Trim());

                beneficio.Descricao = request.Beneficio.Trim();

                await _repository.EditarBeneficioAsync(beneficio);

            }
            catch (OperacaoInvalidaException) {}

            return beneficio;
        }
    }
}