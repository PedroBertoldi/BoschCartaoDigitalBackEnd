namespace BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom
{
    public class ErrorModel
    {
        /// <summary>
        /// Nome do campo que originou o problema, pode ser nulo.
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Mensagem de erro.
        /// </summary>
        public string Message { get; set; }

        public ErrorModel()
        {

        }
        public ErrorModel(string msg, string field = "")
        {
            Message = msg;
            FieldName = field;
        }
    }
}