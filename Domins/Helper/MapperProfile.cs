using AutoMapper;
using Domins.Dtos.Auth_dto;
using Domins.Dtos.Dto;
using Domins.Model;

namespace Domins.Helper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Patient, PatinetDestinationDto>();
            CreateMap<Alarm, Alarmdto>();
            CreateMap<RegisterAsPatientRequest,Patient>();

        }
    }
}

