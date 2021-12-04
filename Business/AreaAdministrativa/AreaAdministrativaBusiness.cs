using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Exceptions.Commom;

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

        public async Task<Evento> BuscarEventoIdAsync(int id)
        {
            Evento evento = await _repository.BuscarEventoPorIdAsync(id);
            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"O Evento com o ID : {id} não foi encontrado",
                });
                throw new OperacaoInvalidaException();
            }
            return evento;
        }

        public async Task<List<Beneficio>> ListaBeneficiosPorEventoAsync(int id)
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

        private async Task ValidarRelacionamentoBeneficioEventoIdBeneficioAsync(int beneficioId)
        {
            BeneficioEvento beneficioEvento = await _repository.BuscarBeneficioEventoIdBeneficioAsync(beneficioId);
            if(beneficioEvento != null){
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(beneficioId),
                    Message = $"Existe um BeneficioEvento com o este beneficioId: {beneficioId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        private async Task ValidarRelacionamentoBeneficioEventoIdEventoAsync(int eventoId)
        {
            List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdEventoAsync(eventoId);
            if(beneficiosEvento != null){
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(eventoId),
                    Message = $"Existe um BeneficioEvento com o este eventoId: {eventoId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        private async Task ValidarRelacionamentoDireitoIdBeneficioAsync(int beneficioId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdBeneficioAsync(beneficioId);
            if(direitos != null){
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(beneficioId),
                    Message = $"Existe um Direito com o este beneficioId: {beneficioId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        private async Task ValidarRelacionamentoDireitoIdEventoAsync(int eventoId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdEventoAsync(eventoId);
            if(direitos != null){
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(eventoId),
                    Message = $"Existe um Direito com o este eventoId: {eventoId}.",
                });
                throw new OperacaoInvalidaException();
            }
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

        public async Task ExcluirBeneficioEventoIdBeneficioAsync(int beneficioId)
        {
            BeneficioEvento beneficioEvento = await _repository.BuscarBeneficioEventoIdBeneficioAsync(beneficioId);
            if(beneficioEvento != null)
            {
                await _repository.ExcluirBeneficioEventoAsync(beneficioEvento);
            }
            else
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(beneficioEvento),
                    Message = $"Não existe um BeneficioEvento com o seguinte beneficioId: {beneficioId}"
                });
            }
        }

        public async Task ExcluirBeneficioEventoIdEventoAsync(int eventoId)
        {
            List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdEventoAsync(eventoId);
            if(beneficiosEvento.Count > 0)
            {
                foreach (BeneficioEvento beneficioEvento in beneficiosEvento)
                {
                    await _repository.ExcluirBeneficioEventoAsync(beneficioEvento);
                }
            }
            else
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(beneficiosEvento),
                    Message = $"Não existe um BeneficioEvento com o seguinte eventoId: {eventoId}"
                });
            }
        }

        public async Task ExcluirDireitoIdBeneficioAsync(int beneficioId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdBeneficioAsync(beneficioId);
            if(direitos.Count > 0)
            {
                foreach (Direito direito in direitos)
                {
                    await _repository.ExcluirDireitoAsync(direito);
                }
            }
            else
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(direitos),
                    Message = $"Não existe um Direito com o seguinte beneficioId: {beneficioId}"
                });
            }
        }

        public async Task ExcluirDireitoIdEventoAsync(int eventoId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdEventoAsync(eventoId);
            if(direitos.Count > 0)
            {
                foreach (Direito direito in direitos)
                {
                    await _repository.ExcluirDireitoAsync(direito);
                }
            }
            else
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(direitos),
                    Message = $"Não existe um Direito com o seguinte eventoId: {eventoId}"
                });
            }
        }

        public async Task ExcluirBeneficioAsync(int id)
        {
            try
            {
                Beneficio beneficio = await BuscarBeneficioPorIdAsync(id);
                await ValidarRelacionamentoBeneficioEventoIdBeneficioAsync(id);
                await ValidarRelacionamentoDireitoIdBeneficioAsync(id);

                await _repository.ExcluirBeneficioAsync(beneficio);
            }
            catch (OperacaoInvalidaException) {}
        }

        public async Task ExcluirEventoIdAsync(int id)
        {
            try
            {
                Evento evento = await BuscarEventoIdAsync(id);
                await ValidarRelacionamentoBeneficioEventoIdEventoAsync(id);
                await ValidarRelacionamentoDireitoIdEventoAsync(id);

                await _repository.ExcluirEventoAsync(evento);
            }
            catch (OperacaoInvalidaException) {}
        }

        public async Task ExcluirEventoIdCascataAsync(int id)
        {
            Evento evento = default;
            try
            {
                evento = await BuscarEventoIdAsync(id);
            }
            catch (OperacaoInvalidaException) {}

            try
            {
                await ExcluirBeneficioEventoIdEventoAsync(id);
                await ExcluirDireitoIdEventoAsync(id);

                await _repository.ExcluirEventoAsync(evento);
            }
            catch {}
        }

        public async Task<List<Beneficio>> BuscarTodosBeneficiosAsync()
        {
            return await _repository.BuscarTodosBeneficiosAsync();
        }

        public async Task<Beneficio> BuscarUnicoBeneficioPorIdAsync(int id)
        {
            Beneficio retorno = default;
            try
            {
                retorno = await BuscarBeneficioPorIdAsync(id);
            }
            catch (OperacaoInvalidaException) { }
            return retorno;
        }

        public async Task<BeneficioEvento> CriarEAtrelarBeneficioAsync(CriarEAtrelarBeneficioRequest request)
        {
            var beneficio = await CadastrarBeneficioAsync(new CriarEditarBeneficioRequest { Beneficio = request.Beneficio });
            var beneficioEvento = await CriarRelacaoBeneficioEventoAsync(new RelacaoBeneficioEventoRequest { BeneficioId = beneficio.Id, EventoId = request.EventoId });
            return beneficioEvento;
        }
    }
}