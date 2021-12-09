using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response;
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
        //Recebe a id de um evento e retorna um objeto evento referente ao titular da ID
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
            //Recebe o ID de um evento e retorna uma lista de objetos beneficio referente aos benecificios do evento em questão
            List<Beneficio> lista = default;
            if (id == 0)
            {
                lista = await _repository.ListaBeneficioEventoAtivoAsync();
                if (lista == null)
                    lista = await _repository.ListaBeneficioProximoEventoAsync();
            }
            else
            {
                lista = await _repository.ListaBeneficioIdEventoAsync((int)id);
            }

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
            if (fim.Date < inicio.Date)
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

        /// <summary>
        /// Função que cadastra um beneficiario e seus direitos.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userID">Id do colaborador que realizou o cadastro</param>
        /// <returns></returns>
        public async Task<BeneficiarioResponse> CadastrarBeneficiarioAsync(CriarEditarBeneficiarioRequest request, int userID)
        {
            try
            {
                await ValidarUnidadeOrganizacionalIdAsync((int)request.UnidadeOrganizacionalId);
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }
            

            List<CriarEditarBeneficiarioDireitoResponseResumido> beneficiosResponse = new List<CriarEditarBeneficiarioDireitoResponseResumido>();
            Colaborador colaborador = await _repository.BuscarColaboradorCpfEdvDataNascimentoAsync(request.Cpf, request.Edv, (DateTime)request.DataNascimento);
            if (colaborador != null)
            {
                colaborador.NomeCompleto = request.NomeCompleto;
                colaborador.UnidadeOrganizacionalId = request.UnidadeOrganizacionalId;

                await _repository.EditarColaborador(colaborador);
            }
            else
            {
                colaborador = new Colaborador
                {
                    Cpf = request.Cpf,
                    NomeCompleto = request.NomeCompleto,
                    DataNascimento = request.DataNascimento,
                    UnidadeOrganizacionalId = request.UnidadeOrganizacionalId,
                    Edv = request.Edv,
                    OrigemId = userID,
                    DataDeCadastro = DateTime.Now,
                    
                };
                //VERIFICAR COM O GRUPO SE VAI SER NECESSARIO FAZER UMA VALIDAÇÃO ANTES DE TENTAR INSERIR UM NOVO COLABORADOR
                await _repository.CadastrarNovoColaborador(colaborador);
            }

            if (request.beneficios.Count > 0)
            {
                foreach (CriarEditarBeneficiarioDireitoRequestResumido beneficio in request.beneficios)
                {
                    try
                    {
                        await ValidarEventoIdAsync((int)beneficio.EventoId);
                        await ValidarBeneficioIdAsync((int)beneficio.BeneficioId);
                    }
                    catch (OperacaoInvalidaException)
                    {
                        return null;
                    }

                    if (beneficio.QtdBeneficio > 0)
                    {
                        for (int i = 0; i < beneficio.QtdBeneficio; i++)
                        {
                            Direito direito = new Direito
                            {
                                ColaboradorId = colaborador.Id,
                                EventoId = (int)beneficio.EventoId,
                                BeneficioId = (int)beneficio.BeneficioId,
                            };

                            //fazer tratamento para se não conseguir criar um direito
                            await _repository.CadastrarDireito(direito);
                        }
                    }

                    beneficiosResponse.Add(new CriarEditarBeneficiarioDireitoResponseResumido
                    {
                        EventoId = (int)beneficio.EventoId,
                        BeneficioId = (int)beneficio.BeneficioId,
                        QtdBeneficio = (int)beneficio.QtdBeneficio
                    });
                }
            }

            var temp = new BeneficiarioResponse
            {
                Id = colaborador.Id,
                Cpf = colaborador.Cpf,
                NomeCompleto = colaborador.NomeCompleto,
                DataNascimento = colaborador.DataNascimento,
                UnidadeOrganizacionalId = colaborador.UnidadeOrganizacionalId,
                Edv = colaborador.Edv,
                beneficios = beneficiosResponse,
            };

            return temp;
        }

        /// <summary>
        /// Função que edita um beneficiario e recria seus direitos.
        /// </summary>
        /// <param name="id">Id do colaborador que sera editado</param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BeneficiarioResponse> EditarBeneficiarioAsync(int id, CriarEditarBeneficiarioRequest request)
        {
            try
            {
                await ValidarUnidadeOrganizacionalIdAsync((int)request.UnidadeOrganizacionalId);
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }

            Colaborador colaborador = await _repository.BuscarColaboradorPorIdAsync(id);
            colaborador.Cpf = request.Cpf;
            colaborador.NomeCompleto = request.NomeCompleto;
            colaborador.DataNascimento = request.DataNascimento;
            colaborador.UnidadeOrganizacionalId = request.UnidadeOrganizacionalId;
            colaborador.Edv = request.Edv;

            //VERIFICAR COM O GRUPO SE VAI SER NECESSARIO FAZER UMA VALIDAÇÃO ANTES DE TENTAR ATUALIZAR UM COLABORADOR
            await _repository.EditarColaborador(colaborador);

            List<CriarEditarBeneficiarioDireitoResponseResumido> beneficiosResponse = new List<CriarEditarBeneficiarioDireitoResponseResumido>();

            if (request.beneficios.Count > 0)
            {
                foreach (CriarEditarBeneficiarioDireitoRequestResumido beneficio in request.beneficios)
                {
                    try
                    {
                        await ValidarEventoIdAsync((int)beneficio.EventoId);
                        await ValidarBeneficioIdAsync((int)beneficio.BeneficioId);
                    }
                    catch (OperacaoInvalidaException)
                    {
                        return null;
                    }

                    //REMOVENDO TODOS OS DIREITOS PARA INSERIR NOVOS QUE ESTAO ATUALIZADOS
                    List<Direito> direitos = await _repository.BuscarDireitoIdColaboradorIdEventoIdBeneficioAsync(colaborador.Id, (int)beneficio.EventoId, (int)beneficio.BeneficioId);
                    if (direitos.Count > 0)
                    {
                        foreach (Direito direito in direitos)
                        {
                            await _repository.ExcluirDireitoAsync(direito);
                        }
                    }
                    //REMOVENDO TODOS OS DIREITOS PARA INSERIR NOVOS QUE ESTAO ATUALIZADOS

                    if (beneficio.QtdBeneficio > 0)
                    {
                        for (int i = 0; i < beneficio.QtdBeneficio; i++)
                        {
                            Direito direito = new Direito
                            {
                                ColaboradorId = colaborador.Id,
                                EventoId = (int)beneficio.EventoId,
                                BeneficioId = (int)beneficio.BeneficioId,
                            };

                            //fazer tratamento para se não conseguir criar um direito
                            await _repository.CadastrarDireito(direito);
                        }
                    }

                    beneficiosResponse.Add(new CriarEditarBeneficiarioDireitoResponseResumido
                    {
                        EventoId = (int)beneficio.EventoId,
                        BeneficioId = (int)beneficio.BeneficioId,
                        QtdBeneficio = (int)beneficio.QtdBeneficio
                    });
                }
            }

            var temp = new BeneficiarioResponse
            {
                Id = colaborador.Id,
                Cpf = colaborador.Cpf,
                NomeCompleto = colaborador.NomeCompleto,
                DataNascimento = colaborador.DataNascimento,
                UnidadeOrganizacionalId = colaborador.UnidadeOrganizacionalId,
                Edv = colaborador.Edv,
                beneficios = beneficiosResponse,
            };

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

        /// <summary>
        /// Função interna que verifica se existe algum beneficioEvento com o beneficioId passado.
        /// </summary>
        /// <param name="beneficioId">beneficioId que sera consultado</param>
        private async Task ValidarRelacionamentoBeneficioEventoIdBeneficioAsync(int beneficioId)
        {
            List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdBeneficioAsync(beneficioId);
            if (beneficiosEvento.Count > 0)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(beneficioId),
                    Message = $"Existe um BeneficioEvento com o este beneficioId: {beneficioId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se existe algum beneficioEvento com o eventoId passado.
        /// </summary>
        /// <param name="eventoId">eventoId que sera consultado</param>
        private async Task ValidarRelacionamentoBeneficioEventoIdEventoAsync(int eventoId)
        {
            List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdEventoAsync(eventoId);
            if (beneficiosEvento.Count > 0)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(eventoId),
                    Message = $"Existe um BeneficioEvento com o este eventoId: {eventoId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se existe algum direito com o beneficioId passado.
        /// </summary>
        /// <param name="beneficioId">beneficioId que sera consultado</param>
        private async Task ValidarRelacionamentoDireitoIdBeneficioAsync(int beneficioId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdBeneficioAsync(beneficioId);
            if (direitos.Count > 0)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(beneficioId),
                    Message = $"Existe um Direito com o este beneficioId: {beneficioId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se existe algum direito com o eventoId passado.
        /// </summary>
        /// <param name="eventoId">eventoId que sera consultado</param>
        private async Task ValidarRelacionamentoDireitoIdEventoAsync(int eventoId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdEventoAsync(eventoId);
            if (direitos.Count > 0)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(eventoId),
                    Message = $"Existe um Direito com o este eventoId: {eventoId}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se não existe nenhuma unidade organizacional com o id passado.
        /// </summary>
        /// <param name="id">id da unidade organizacional que sera consultado</param>
        private async Task ValidarUnidadeOrganizacionalIdAsync(int id)
        {
            UnidadeOrganizacional unidadeOrganizacional = await _repository.BuscarUnidadeOrganizacionalIdAsync(id);
            if (unidadeOrganizacional == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"Não existe nenhuma Unidade Organizacional com o este Id: {id}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se não existe nenhum evento com o id passado.
        /// </summary>
        /// <param name="id">id do evento que sera consultado</param>
        private async Task ValidarEventoIdAsync(int id)
        {
            Evento evento = await _repository.BuscarEventoPorIdAsync(id);
            if (evento == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"Não existe nenhum Evento com o este Id: {id}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se não existe nenhum beneficio com o id passado.
        /// </summary>
        /// <param name="id">id do beneficio que sera consultado</param>
        private async Task ValidarBeneficioIdAsync(int id)
        {
            Beneficio beneficio = await _repository.BuscarBeneficioPorIdAsync(id);
            if (beneficio == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(id),
                    Message = $"Não existe nenhum Beneficio com o este Id: {id}.",
                });
                throw new OperacaoInvalidaException();
            }
        }

        /// <summary>
        /// Função interna que verifica se não existe nenhum beneficio com o id passado.
        /// </summary>
        /// <param name="id">id do beneficio que sera consultado</param>
        /// <returns></returns>
        private async Task<Beneficio> BuscarBeneficioPorIdAsync(int id)
        {
            //Recebe a ID de um beneficio e retorna um objeto Beneficio referente ao titular da id
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

        /// <summary>
        /// Função que edita um beneficio.
        /// </summary>
        /// <param name="id">id do beneficio que sera editado</param>
        /// <param name="request"></param>
        /// <returns></returns>
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
            catch (OperacaoInvalidaException) { }

            return beneficio;
        }

        /// <summary>
        /// Função que exclui todos os vinculos de beneficio com o evento pelo beneficioId.
        /// </summary>
        /// <param name="beneficioId">id do beneficio que sera usado para a exclusão</param>
        public async Task ExcluirBeneficioEventoIdBeneficioAsync(int beneficioId)
        {
            List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdBeneficioAsync(beneficioId);
            if (beneficiosEvento.Count > 0)
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
                    Message = $"Não existe um BeneficioEvento com o seguinte beneficioId: {beneficioId}"
                });
            }
        }

        /// <summary>
        /// Função que exclui todos os vinculos de beneficio com o evento pelo eventoId.
        /// </summary>
        /// <param name="eventoId">id do evento que sera usado para a exclusão</param>
        public async Task ExcluirBeneficioEventoIdEventoAsync(int eventoId)
        {
            List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdEventoAsync(eventoId);
            if (beneficiosEvento.Count > 0)
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

        /// <summary>
        /// Função que exclui todos os direitos pelo beneficioId.
        /// </summary>
        /// <param name="beneficioId">id do beneficio que sera usado para a exclusão</param>
        public async Task ExcluirDireitoIdBeneficioAsync(int beneficioId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdBeneficioAsync(beneficioId);
            if (direitos.Count > 0)
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

        /// <summary>
        /// Função que exclui todos os direitos pelo eventoId.
        /// </summary>
        /// <param name="eventoId">id do evento que sera usado para a exclusão</param>
        public async Task ExcluirDireitoIdEventoAsync(int eventoId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdEventoAsync(eventoId);
            if (direitos.Count > 0)
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

        /// <summary>
        /// Função que exclui todos os direitos que contenham o colaboradorId e o eventoId no mesmo registro.
        /// </summary>
        /// <param name="colaboradorId">id do colaborador que sera usado para a exclusão</param>
        /// <param name="eventoId">id do evento que sera usado para a exclusão</param>
        public async Task ExcluirDireitoIdColaboradorIdEventoAsync(int colaboradorId, int eventoId)
        {
            List<Direito> direitos = await _repository.BuscarDireitoIdColaboradorIdEventoAsync(colaboradorId, eventoId);
            if (direitos.Count > 0)
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
                    Message = $"Não existe nenhum Direito que tenha o colaboradorId: {colaboradorId}, e o eventoId: {eventoId}."
                });
            }
        }

        /// <summary>
        /// Função que exclui um beneficio pelo id desde que este beneficio não tenha vinculos com evento ou direito.
        /// </summary>
        /// <param name="id">id do beneficio que sera usado para a exclusão</param>
        public async Task ExcluirBeneficioAsync(int id)
        {
            try
            {
                Beneficio beneficio = await BuscarBeneficioPorIdAsync(id);
                await ValidarRelacionamentoBeneficioEventoIdBeneficioAsync(id);
                await ValidarRelacionamentoDireitoIdBeneficioAsync(id);

                await _repository.ExcluirBeneficioAsync(beneficio);
            }
            catch (OperacaoInvalidaException) { }
        }

        /// <summary>
        /// Função que exclui um beneficio pelo id e excluir todos os relacionamentos com evento ou direito que este beneficio tenha.
        /// </summary>
        /// <param name="id">id do beneficio que sera usado para a exclusão</param>
        public async Task ExcluirBeneficioCascataAsync(int id)
        {
            try
            {
                Beneficio beneficio = await BuscarBeneficioPorIdAsync(id);

                List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdBeneficioAsync(id);
                if (beneficiosEvento.Count > 0)
                {
                    await ExcluirBeneficioEventoIdBeneficioAsync(id);
                }

                List<Direito> direitos = await _repository.BuscarDireitoIdBeneficioAsync(id);
                if (direitos.Count > 0)
                {
                    await ExcluirDireitoIdBeneficioAsync(id);
                }

                await _repository.ExcluirBeneficioAsync(beneficio);
            }
            catch (OperacaoInvalidaException) { }
        }

        /// <summary>
        /// Função que exclui um evento pelo id desde que este evento não tenha vinculos com beneficio ou direito.
        /// </summary>
        /// <param name="id">id do evento que sera usado para a exclusão</param>
        public async Task ExcluirEventoIdAsync(int id)
        {
            try
            {
                Evento evento = await BuscarEventoIdAsync(id);
                await ValidarRelacionamentoBeneficioEventoIdEventoAsync(id);
                await ValidarRelacionamentoDireitoIdEventoAsync(id);

                await _repository.ExcluirEventoAsync(evento);
            }
            catch (OperacaoInvalidaException) { }
        }

        /// <summary>
        /// Função que exclui um evento pelo id e excluir todos os relacionamentos com beneficio ou direito que este evento tenha.
        /// </summary>
        /// <param name="id">id do evento que sera usado para a exclusão</param>
        public async Task ExcluirEventoIdCascataAsync(int id)
        {
            try
            {
                Evento evento = await BuscarEventoIdAsync(id);

                List<BeneficioEvento> beneficiosEvento = await _repository.BuscarBeneficioEventoIdEventoAsync(id);
                if (beneficiosEvento.Count > 0)
                {
                    await ExcluirBeneficioEventoIdEventoAsync(id);
                }

                List<Direito> direitos = await _repository.BuscarDireitoIdEventoAsync(id);
                if (direitos.Count > 0)
                {
                    await ExcluirDireitoIdEventoAsync(id);
                }

                await _repository.ExcluirEventoAsync(evento);
            }
            catch (OperacaoInvalidaException) { }
        }
        /// <summary>
        /// Retorna todos os Beneficios existentes
        /// </summary>
        /// <returns></returns>
        public async Task<List<Beneficio>> BuscarTodosBeneficiosAsync()
        {
            //Retorna uma lista que contém todos os benefícios
            return await _repository.BuscarTodosBeneficiosAsync();
        }
        /// <summary>
        /// Retorna um único beneficio pelo seu ID
        /// </summary>
        /// <param name="id">ID do beneficio</param>
        /// <returns></returns>        
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
        /// <summary>
        /// Cria um novo tipo de beneficio e atrela a um evento
        /// </summary>
        /// <param name="request">Parametros necessários</param>
        /// <returns></returns>
        public async Task<BeneficioEvento> CriarEAtrelarBeneficioAsync(CriarEAtrelarBeneficioRequest request)
        {
            var beneficio = await CadastrarBeneficioAsync(new CriarEditarBeneficioRequest { Beneficio = request.Beneficio });
            var beneficioEvento = await CriarRelacaoBeneficioEventoAsync(new RelacaoBeneficioEventoRequest { BeneficioId = beneficio.Id, EventoId = request.EventoId });
            return beneficioEvento;
        }
        /// <summary>
        /// Busca um colaborador por ID, caso não encontrado produz uma exceção
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Colaborador> BuscarColaboradorPorIdAsync(int id)
        {
            //Recebe a id de um colaborador e retorna um objeto Colaborador referente ao titular da id
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
        /// Busca os direitos por colaborador agrupado.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DireitosPorColaboradorAgrupadosADM> BuscarDireitosPorIdColaboradorAsync(DireitosColaboradorRequest request)
        {
            //Recebe uma request que contém o id de um evento e o id de um colaborador e retorna uma lista com informações
            //Sobre os direitos do colaborador e o indicado sugerido por ele, se existente
            DireitosPorColaboradorAgrupadosADM resposta = default;
            try
            {
                var colab = await BuscarColaboradorPorIdAsync((int)request.idColaborador);
                var evento = await BuscarEventoIdAsync((int)request.EventoID);
                var indicado = await _repository.BuscarIndicado((int)request.EventoID, (int)request.idColaborador);
                var direitos = await _repository.BuscarDireitosPorIdColaboradorAsync((int)request.EventoID, (int)request.idColaborador);
                resposta = new DireitosPorColaboradorAgrupadosADM
                {
                    Colaborador = colab,
                    Evento = evento,
                    Indicado = indicado,

                };
                if (direitos == null)
                {
                    resposta.Direitos = new List<Direito>();
                }
                else
                {
                    resposta.Direitos = direitos;
                }
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }
            return resposta;
        }
        /// <summary>
        /// Busca todos os direitos de todos colaboradores em um evento
        /// </summary>
        /// <param name="idEvento"></param>
        /// <returns></returns>
        public async Task<DireitosTodosColaboradoresAgrupados> BuscarTodosDireitosPorColaborador(int idEvento)
        {
            //Recebe o ID de um evento e retorna uma lista com a seguinte estrutura:
            //[EVENTO, [Beneficio1, [Beneficiario1, Beneficiario2]], [Beneficio2, [Beneficiario1, Beneficiario3],...]
            var resposta = new DireitosTodosColaboradoresAgrupados();
            try
            {
                resposta.Evento = await BuscarEventoIdAsync(idEvento); //Associa o evento ao objeto a partir do id do evento
                resposta.ColaboradoresDireitos = new List<DireitosColaboradorAgrupadosSemEvento>();  //instancia o objeto que contém 1 colaborador e seus direitos
                var colaboradores = await _repository.BuscarTodosColaboradoresBosch(); //Obtém a lista de todos os colaboradores
                foreach (Colaborador c in colaboradores) //Itera por cada colaborador
                {
                    var idC = c.Id;
                    var indicado = await _repository.BuscarIndicado(idEvento, idC);
                    var direitos = await _repository.BuscarDireitosPorIdColaboradorAsync(idEvento, idC);
                    if (direitos.Count > 0)
                    { //Se o colaborador possui pelo menos 1 direito, coloca ele na resposta
                        var direitosSalvar = new DireitosColaboradorAgrupadosSemEvento
                        {
                            Colaborador = c,
                            Direitos = direitos,
                            Indicado = indicado,
                        };
                        resposta.ColaboradoresDireitos.Add(direitosSalvar);
                    }

                }
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }

            return resposta;
        }
        /// <summary>
        /// Retorna todas as unidades organizacionais
        /// </summary>
        /// <returns></returns>
        public async Task<List<UnidadeOrganizacional>> listarUnidadeOrganizacionalAsync()
        {
            return await _repository.listarUnidadeOrganizacional();
        }
        /// <summary>
        /// busca um colaborador pelo seu código EDV
        /// </summary>
        /// <param name="edv"></param>
        /// <returns></returns>
        private async Task<Colaborador> BuscarColaboradorPorEDVsync(string edv)
        {
            //Recebe o EDV de um colaborador e retorna um objeto Colaborador referente ao titular do EDV
            var colaborador = await _repository.BuscarColaboradorPorEDV(edv);
            if (colaborador == null)
            {
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(edv),
                    Message = $"Não foi encontrado um colaborador com o EDV: {edv}",
                });
                throw new OperacaoInvalidaException();
            }
            return colaborador;
        }
        /// <summary>
        /// Busca os direitos de um colaborador em um evento pelo seu edv
        /// </summary>
        /// <param name="idEvento"></param>
        /// <param name="edv"></param>
        /// <returns></returns>
        public async Task<DireitosPorColaboradorAgrupadosADM> BuscarDireitosPorEDVColaboradorAsync(int idEvento, string edv)
        {
            //Recebe a ID de um evento e EDV de um colaborador e retorna todos os direitos do colaborador naquele evento
            DireitosPorColaboradorAgrupadosADM resposta = default;
            try
            {
                var colab = await BuscarColaboradorPorEDVsync(edv);
                var evento = await BuscarEventoIdAsync(idEvento);
                var indicado = await _repository.BuscarIndicado(idEvento, colab.Id);
                var direitos = await _repository.BuscarDireitosPorIdColaboradorAsync(idEvento, colab.Id);
                resposta = new DireitosPorColaboradorAgrupadosADM
                {
                    Colaborador = colab,
                    Evento = evento,
                    Indicado = indicado,
                };
                if (direitos == null)
                {
                    resposta.Direitos = new List<Direito>();
                }
                else
                {
                    resposta.Direitos = direitos;
                }
            }
            catch (OperacaoInvalidaException)
            {
                return null;
            }
            return resposta;
        }
    }
}