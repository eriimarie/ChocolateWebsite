using AutoMapper;
using ChocoShop.Data.Entities;
using ChocoShop.Extensions;
using ChocoShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Mapper
{
    public class RequestToDomainMappingProfile : Profile
    {
        public RequestToDomainMappingProfile()
        {
            CreateMap<RegistrationModel, User>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                 .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.ToHash()))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}
