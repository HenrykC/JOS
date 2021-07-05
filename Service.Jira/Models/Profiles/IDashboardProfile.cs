using System.Collections.Generic;

namespace Service.Jira.Models.Profiles
{
    public interface IDashboardProfile
    {
        int BoardId { get; set; }
        int DashBoardId { get; set; }
        IList<GadgetProfile> GadgetProfiles { get; set; }
        string JiraServer { get; set; }
    }
}