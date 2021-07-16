using Global.Models.Profiles;

namespace Service.Produktteam.Capacity
{
    public interface IOutlookConnectionProfile : IConnectionProfile
    {
        public string MailServerUrl { get; set; }
        public bool AutodiscoverUrl { get; set; }
    }
}
