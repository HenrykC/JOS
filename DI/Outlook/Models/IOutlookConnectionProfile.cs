namespace DI.Outlook.Models
{
    public interface IOutlookConnectionProfile
    {
        string Domain { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
    }
}