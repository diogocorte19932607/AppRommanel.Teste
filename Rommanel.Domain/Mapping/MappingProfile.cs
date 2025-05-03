using AutoMapper;
using Rommanel.Domain.Domain;
using Rommanel.Infra.Entity;

namespace Rommanel.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Logradouro, LogradouroModel>().ReverseMap();
            CreateMap<ClienteModel, Cliente>().ReverseMap();

        }
    }
}
