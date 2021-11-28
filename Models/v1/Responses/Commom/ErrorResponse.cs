using System.Collections.Generic;

namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom
{
    public class ErrorResponse
    {
        /// <summary>
        /// Lista de erros.
        /// </summary>
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
        public ErrorResponse()
        {

        }
        public ErrorResponse(ErrorModel error)
        {
            Errors.Add(error);
        }
        public ErrorResponse(List<ErrorModel> errors)
        {
            Errors.AddRange(errors);
        }
        public ErrorResponse(string msg, string field = "")
        {
            Errors.Add(new ErrorModel(msg, field));
        }
    }
}