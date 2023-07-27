using AutoMapper;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.Application.Mappings
{
    public class SchoolNetAssociationProfile : Profile
    {
        public SchoolNetAssociationProfile()
        {
            CreateMap<SchoolDistrict, SchoolDistrictDto>().ReverseMap();
        }
    }
}
