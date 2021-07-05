using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Jira.Models.Profiles
{
    public class DashboardProfile : IDashboardProfile
    {
        public int BoardId { get; set; }
        public int DashBoardId { get; set; }
        public IList<GadgetProfile> GadgetProfiles { get; set; }

        public string JiraServer { get; set; }
    }
}
