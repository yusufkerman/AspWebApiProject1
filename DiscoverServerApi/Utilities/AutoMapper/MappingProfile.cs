using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace MyWebServerProject.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistirationInformationDto, User>();
        }
    }
}
