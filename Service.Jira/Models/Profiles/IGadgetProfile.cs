namespace Service.Jira.Models.Profiles
{
    public interface IGadgetProfile
    {
        int GadgetId { get; set; }
        string GadgetName { get; set; }
        string SprintNameFilter { get; set; }
    }
}