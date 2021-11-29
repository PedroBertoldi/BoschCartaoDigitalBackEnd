using AutoMapper;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;
using BoschCartaoDigitalBackEnd.Models.v1.Responses.Commom;

namespace BoschCartaoDigitalBackEnd.MappingProfiles
{
    public class DomaniToResponseProfile : Profile
    {
        public DomaniToResponseProfile()
        {
            CreateMap<Direito, DireitoResponse>();
            CreateMap<Colaborador, ColaboradorResponse>();
            CreateMap<Beneficio, BeneficioResponse>();
            CreateMap<Evento, EventoResponse>();
            CreateMap<UnidadeOrganizacional, UnidadeOrganizacionalResponse>();
        }
    }
}