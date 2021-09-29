using System;
using System.Collections.Generic;

namespace Service.Produktteam.Models
{
    public class TeamSetting
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool CreateReport { get; set; }
        public int ActualSprintGadgetId { get; set; }
        public int NextSprintGadgetId { get; set; }

        public string TeamName { get; set; }
        public string FilterSprintName { get; set; }

        public List<string> Email { get; set; }
        public int SprintLength { get; set; }
    }

    public class Settings
    {
        public List<TeamSetting> TeamSettings { get; set; }
    }
}
