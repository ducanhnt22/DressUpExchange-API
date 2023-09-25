﻿using AutoMapper;
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
            CreateMap<CustomerRequest, UserResponse>().ReverseMap();
            CreateMap<RegisterRequest, User>().ReverseMap();
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
            CreateMap<FeedbackRequest, ProductFeedback>()
                .ForMember(dex => dex.UserId, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dex => dex.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dex => dex.Rating, opt => opt.MapFrom(src => src.Rating))
                .ReverseMap();

            



        }
    }
}
