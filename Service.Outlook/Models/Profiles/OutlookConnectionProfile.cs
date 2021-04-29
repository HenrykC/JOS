namespace Service.Outlook.Models.Profiles
{
    public class OutlookConnectionProfile : IOutlookConnectionProfile
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string MailServerUrl { get; set; }
        public bool AutodiscoverUrl { get; set; }
    }
}
