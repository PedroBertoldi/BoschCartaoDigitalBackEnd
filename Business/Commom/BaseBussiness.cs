using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;

namespace BoschCartaoDigitalBackEnd.Business.Commom
{
    public class BaseBussiness
    {
        protected readonly List<ErrorModel> _errors;
        public BaseBussiness()
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