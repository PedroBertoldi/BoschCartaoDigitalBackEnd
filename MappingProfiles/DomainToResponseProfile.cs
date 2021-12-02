using AutoMapper;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica;
using BoschCartaoDigitalBackEnd.Models.v1.AreaPublica.Response;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

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
        }
    }
}