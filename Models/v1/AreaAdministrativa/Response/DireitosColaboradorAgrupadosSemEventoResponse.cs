using System.Collections.Generic;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response;




namespace BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa
{
    public class DireitosColaboradorAgrupadosSemEventoResponse
    {
        public ColaboradorAlteradoResponse Colaborador { get; set; }

        public string origem { get; set; }

        public ColaboradorResponse Indicado { get; set; }

        public List<DireitoInfoReduzidaResponseADM> Direitos { get; set; }

    }
}