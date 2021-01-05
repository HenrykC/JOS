using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //CreateMap<DI.Outlook.Models.Appointment, Microsoft.Exchange.WebServices.Data.Appointment>();
            CreateMap<Microsoft.Exchange.WebServices.Data.Appointment, DI.Outlook.Models.Appointment>()
                .ForMember(dest => dest.AppointmentId, src => src.MapFrom(src => src.Id.UniqueId.ToString()))
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.AppointmentType, src => src.Ignore());
        }
    }
}