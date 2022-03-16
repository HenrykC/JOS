using AutoMapper;
using Global.Models.Jira;

namespace Service.Jira.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IssueDbModel, Issue>()
                .ForMember(x => x.Estimation, opt => opt.MapFrom(src => src.Fields.Customfield_10106))
                .ForMember(x => x.Changelog, opt => opt.MapFrom(src => src.Changelog))
                .AfterMap((src, dest) =>
                {
                    foreach (var sourcePropertyInfo in src.Fields.GetType().GetProperties())
                    {
                        var destPropertyInfo = dest.GetType().GetProperty(sourcePropertyInfo.Name);

                        destPropertyInfo?.SetValue(
                            dest,
                            sourcePropertyInfo.GetValue(src.Fields, null),
                            null);
                    }
                });

            //CreateMap<IssueDbModel, Issue>()
            //    .ForMember(x => x.Summary, opt => opt.MapFrom(src => src.Fields.Summary))
            //    .ForMember(x => x.Priority, opt => opt.MapFrom(src => src.Fields.Priority))
            //    .ForMember(x => x.Assignee, opt => opt.MapFrom(src => src.Fields.Assignee))
            //    .ForMember(x => x.ClosedSprints, opt => opt.MapFrom(src => src.Fields.ClosedSprints))
            //    .ForMember(x => x.Creator, opt => opt.MapFrom(src => src.Fields.Creator))
            //    .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Fields.Description))
            //    .ForMember(x => x.Created, opt => opt.MapFrom(src => src.Fields.Created))
            //    .ForMember(x => x.Epic, opt => opt.MapFrom(src => src.Fields.Epic))
            //    .ForMember(x => x.Estimation, opt => opt.MapFrom(src => src.Fields.Customfield_10106))
            //    .ForMember(x => x.FixVersions, opt => opt.MapFrom(src => src.Fields.FixVersions))
            //    .ForMember(x => x.Project, opt => opt.MapFrom(src => src.Fields.Project))
            //    .ForMember(x => x.Resolution, opt => opt.MapFrom(src => src.Fields.Resolution))
            //    .ForMember(x => x.Resolutiondate, opt => opt.MapFrom(src => src.Fields.Resolutiondate))
            //    .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Fields.Status))
            //    .ForMember(x => x.Issuetype, opt => opt.MapFrom(src => src.Fields.Issuetype))
            //    .ForMember(x => x.Versions, opt => opt.MapFrom(src => src.Fields.Versions))
            //    .ForMember(x => x.Sprint, opt => opt.MapFrom(src => src.Fields.Sprint));

        }
    }
}
