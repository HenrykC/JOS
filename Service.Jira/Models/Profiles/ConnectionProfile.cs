namespace Service.Jira.Models.Profiles
{
    public class ConnectionProfile : IConnectionProfile
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
