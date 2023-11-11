using AutoMapper;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            CreateMap<Account, AccountDTO>();
        }
    }
}
