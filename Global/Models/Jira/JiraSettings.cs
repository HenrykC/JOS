using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.Jira
{
    public class JiraSettings
    {
        public int JiraBoardId { get; set; }
        public int DashboardId { get; set; }
        public int ActualSprintGadgetId { get; set; }
        public int NextSprintGadgetId { get; set; }
        public int SprintHistoryGadgetId { get; set; }
        public int VelocityGadgetId { get; set; }

        public string FilterSprintName { get; set; }
    }
}
