namespace Service.Jira.Models.Profiles
{
    public class GadgetProfile : IGadgetProfile
    {
        public int GadgetId { get; set; }
        public string GadgetName { get; set; }
        public string SprintNameFilter { get; set; }
    }
}