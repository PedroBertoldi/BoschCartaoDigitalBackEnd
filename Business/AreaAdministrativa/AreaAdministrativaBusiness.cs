using System.Collections.Generic;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Repository.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Business.Commom;

namespace BoschCartaoDigitalBackEnd.Business.AreaAdministrativa
{
    public class AreaAdministrativaBusiness : BaseBussiness
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

        public async Task<List<Beneficio>> ListaBeneficiosAsync(ListarBeneficiosEventoRequest request)
        {
            List<Beneficio> lista = await _repository.ListaBeneficioIdEventoAsync((int)request.eventoId);
            if(lista == null){
                _errors.Add(new ErrorModel
                {
                    FieldName = nameof(request.eventoId),
                    Message = $"Nenhum beneficio encontrado com este eventoId: {(int)request.eventoId}",
                });
                return null;
            }
            return lista;
        }
    }
}