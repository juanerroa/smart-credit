using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Domain.Entities;
using SmartCredit.BackEnd.Application.Helpers;
using SmartCredit.BackEnd.Domain.CustomEntities;

namespace SmartCredit.BackEnd.Application.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
           CreateMap<CreditCard, CreditCardDTO>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName())).ReverseMap();

            CreateMap<Transaction, TransactionDTO>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName()));

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<CreditCardStatement, CreditCardStatementDTO>().ReverseMap();

        }
    }
}