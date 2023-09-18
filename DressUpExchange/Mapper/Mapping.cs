using AutoMapper;
using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;

namespace DressUpExchange.API.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, CustomerRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<CustomerRequest, UserResponse>().ReverseMap();
        }
    }
}
