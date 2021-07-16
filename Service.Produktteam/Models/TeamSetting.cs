using System;
using System.Collections.Generic;

namespace Service.Produktteam.Models
{
    public class TeamSetting
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string TeamName { get; set; }
        public string FilterSprintName { get; set; }

        public List<string> Email { get; set; }
    }

    public class Settings
    {
        public List<TeamSetting> TeamSettings { get; set; }
    }
}
