using System.Collections.Generic;
using System.Threading.Tasks;
using BoschCartaoDigitalBackEnd.Business.Commom;
using BoschCartaoDigitalBackEnd.Database.Context;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Request;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;
using BoschCartaoDigitalBackEnd.Repository.AreaOperacional;

namespace BoschCartaoDigitalBackEnd.Business.AreaOperacional
{
    public class AreaOperacionalBusiness : BaseBusiness
    {
        private readonly AreaOperacionalRepository _repository;

        public AreaOperacionalBusiness(AreaOperacionalRepository repository)
        {
            _repository = repository;
        }

        public async Task BuscarBeneficiosPorCPFOuEDV(BuscarDireitosRequest request)
        {
            var colaborador = (string.IsNullOrEmpty(request.Cpf)) ? await _repository.BuscarColaboradorPorEdvAsync(request.Edv.Trim()) :
                await _repository.BuscarColaboradorPorCPFAsync(request.Cpf.Trim());
            
            if (colaborador == null)
            {
                var cpfNulo = string.IsNullOrEmpty(request.Cpf);
                _errors.Add(new ErrorModel{
                    FieldName = (cpfNulo) ? nameof(request.Edv) : nameof(request.Cpf),
                    Message = $"Não foi possível encontrar colaborador com o {(cpfNulo ? "EDV" : "CPF")} : {(cpfNulo ? request.Edv : request.Cpf)}"
                });
                return;
            }
            
        }
    }
}