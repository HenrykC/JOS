namespace Service.Jira.Models
{
    public interface IConnectionProfile
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Domain { get; set; }
        string Email { get; set; }
        string Url { get; set; }
    }
}