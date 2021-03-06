using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.Commom.Responses;

namespace BoschCartaoDigitalBackEnd.Business.Commom
{
    public class BaseBusiness
    {
        protected readonly List<ErrorModel> _errors;
        public BaseBusiness()
        {
            _errors = new List<ErrorModel>();
        }
        public ErrorResponse BuscarErros(){
            return (_errors.Count > 0) ? new ErrorResponse{
                Errors = _errors,
            } : null;
        }
    }
}