using System;
using System.Collections.Generic;
using Global.Models.Jira;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Service.Produktteam.Models
{
    public class TeamSetting
    {
        public string TeamName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool CreateReport { get; set; }

        public JiraSettings JiraSettings{ get; set; }

        public List<string> Email { get; set; }
        public int SprintLength { get; set; }
    }

    public class Settings
    {
        public List<TeamSetting> TeamSettings { get; set; }
    }
}
