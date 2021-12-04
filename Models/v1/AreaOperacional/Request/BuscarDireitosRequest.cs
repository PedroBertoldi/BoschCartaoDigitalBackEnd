namespace BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Request
{
    public class BuscarDireitosRequest
    {
        /// <summary>
        /// Cpf do colaborador para pesquisar os beneficios, nulo se Edv foi informado.
        /// </summary>
        public string Cpf { get; set; }
        /// <summary>
        /// EDV do colaborador para pesquisar os beneficios, nulo se cpf foi informado.
        /// </summary>
        public string Edv { get; set; }
        /// <summary>
        /// Id do evento a ser pesquisado, se nulo será selecionado o proximo evento ativo ou que será ativo
        /// </summary>
        public int? EventoId { get; set; }
    }
}