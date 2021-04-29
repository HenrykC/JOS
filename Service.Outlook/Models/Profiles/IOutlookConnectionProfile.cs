using Global.Models.Profiles;

namespace Service.Outlook.Models.Profiles
{
    public interface IOutlookConnectionProfile : IConnectionProfile
    {
        public string MailServerUrl { get; set; }
        public bool AutodiscoverUrl { get; set; }
    }
}
