using System.Threading.Tasks;
using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Repository.AreaAdmin;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Models.v1.Request.AreaAdmin;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Exceptions.AreaAdministrativa;

namespace BoschCartaoDigitalBackEnd.Business.AreaAdmin
{

    public class AreaAdminBusiness : BaseBussiness
    {
        private readonly AreaAdminRepository _repository;
        public AreaAdminBusiness(AreaAdminRepository repository) : base()
        {
            _repository = repository;
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