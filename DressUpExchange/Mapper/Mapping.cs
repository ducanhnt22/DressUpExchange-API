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
            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<CustomerRequest, UserResponse>()
                .ForMember(dex => dex.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dex => dex.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dex => dex.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
            CreateMap<RegisterRequest, User>().ReverseMap();

            CreateMap<User, UserLoginResponse>().ReverseMap();
            CreateMap<User, LoginRequest>().ReverseMap();
            CreateMap<LoginRequest, UserLoginResponse>().ReverseMap();

            CreateMap<Product, ProductRequest>().ReverseMap();
            CreateMap<Product, ProductGetRequest>().ReverseMap();
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages))
                .ReverseMap();

            CreateMap<ProductGetRequest, ProductResponse>()
                .ForMember(dex => dex.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dex => dex.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dex => dex.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dex => dex.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dex => dex.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dex => dex.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dex => dex.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dex => dex.Thumbnail, opt => opt.MapFrom(src => src.Thumbnail))
                .ReverseMap();

            CreateMap<ProductResponse, ProductResponse>().ReverseMap();
            CreateMap<ProductRequest, ProductResponse>().ReverseMap();
            CreateMap<ProductImage, ProductImageRequest>().ReverseMap();

            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryRequest>().ReverseMap();

            CreateMap<VoucherRequest, Voucher>()
                .ForMember(dex => dex.UserId, opt => opt.MapFrom(src => src.UserId))
                 .ForMember(dex => dex.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dex => dex.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dex => dex.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
            .ForMember(dex => dex.RemainingCount, opt => opt.MapFrom(src => src.RemainingCount))
            .ForMember(dex => dex.ExpireDate, opt => opt.MapFrom(src => src.ExpireDate))
                .ReverseMap();

            CreateMap<VoucherDetailResponse, Voucher>()
                .ForMember(dex => dex.VoucherId, opt => opt.MapFrom(src => src.voucherId))
                .ForMember(dex => dex.Name, opt => opt.MapFrom(src => src.voucherName))
                .ForMember(dex => dex.Code, opt => opt.MapFrom(src => src.voucherCode))
                .ForMember(dex => dex.DiscountAmount, opt => opt.MapFrom(src => src.discountAmount))
                .ForMember(dex => dex.ExpireDate, opt => opt.MapFrom(src => src.expireDate))
                .ReverseMap();


            }
    }
}
