using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRestFull_Basic_RepositoryPattern
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserForCreationDto, User>();
            CreateMap<UserForUpdateDto, User>();

            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerForCreateDto, Owner>();
            CreateMap<OwnerForUpdateDto, Owner>();

            CreateMap<Account, AccountDto>();

            CreateMap<Shipper, ShipperDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.InteralId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
            CreateMap<ShipperDto, Shipper>()
                .ForMember(dest => dest.InteralId, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Text));
            
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.InteralId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.InteralId, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Text));
        }
    }
}
