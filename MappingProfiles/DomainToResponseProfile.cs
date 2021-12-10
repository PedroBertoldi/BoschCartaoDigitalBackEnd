using AutoMapper;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa;
using BoschCartaoDigitalBackEnd.Models.v1.AreaAdministrativa.Response;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional.Response;
using BoschCartaoDigitalBackEnd.Models.v1.AreaOperacional;

namespace BoschCartaoDigitalBackEnd.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Direito, DireitoResponse>();
            CreateMap<Colaborador, ColaboradorResponse>();
            CreateMap<Beneficio, BeneficioResponse>();
            CreateMap<Evento, EventoResponse>();
            CreateMap<UnidadeOrganizacional, UnidadeOrganizacionalResponse>();
            CreateMap<Direito, DireitoInfoReduzidaResponse>();
            CreateMap<DireitosPorColaboradorAgrupados, DireitosPorColaboradorAgrupadosResponse>();
            CreateMap<Beneficio, ListarBeneficiosEventoResponse>();
            CreateMap<BeneficioEvento, BeneficioEventoResponse>();
            CreateMap<DireitosAgrupadosAoColaborador, DireitosAgrupadosAoColaboradorResponse>();
            CreateMap<Colaborador, ColaboradorResponseResumida>();
            CreateMap<Direito, DireitoResponseResumido>();

            CreateMap<DireitosPorColaboradorAgrupadosADM, DireitosPorColaboradorAgrupadosResponseADM>();
            CreateMap<Direito, DireitoInfoReduzidaResponseADM>();

            CreateMap<DireitosTodosColaboradoresAgrupados, DireitosTodosColaboradoresAgrupadosResponse>();
            CreateMap<DireitosColaboradorAgrupadosSemEvento, DireitosColaboradorAgrupadosSemEventoResponse>();

            CreateMap<ColaboradoresAgrupadosAoDireito, ColaboradoresAgrupadosAoDireitoResponse>();
            CreateMap<BeneficiarioResumido, BeneficiarioResumidoResponse>();

            CreateMap<ColaboradorAlterado, ColaboradorAlteradoResponse>();





        }
    }
}