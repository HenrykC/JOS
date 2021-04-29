using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Service.Outlook.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //CreateMap<DI.Outlook.Models.Appointment, Microsoft.Exchange.WebServices.Data.Appointment>();
            CreateMap<Microsoft.Exchange.WebServices.Data.Appointment, Service.Outlook.Models.Appointment>()
                .ForMember(dest => dest.AppointmentId, src => src.MapFrom(src => src.Id.UniqueId.ToString()))
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.Duration, src => src.MapFrom(src => src.End.Subtract(src.Start)))
                .ForMember(dest => dest.Organizer, src => src.Ignore())
                .ForMember(dest => dest.AppointmentType, src => src.Ignore())
                .ForMember(dest => dest.IsRecurring, src => src.Ignore())
                .ForMember(dest => dest.IsCancelled, src => src.Ignore())
                .ForMember(dest => dest.IsAllDayEvent, src => src.Ignore())
                .ForMember(dest => dest.IsMeeting, src => src.Ignore())
                ;
            //.ForMember(dest => dest.Organizer, src => src.MapFrom(src => src.Organizer.Name))
            //.ForMember(dest => dest.AppointmentType, src => src.MapFrom(src => src.AppointmentType))
        }
    }
}