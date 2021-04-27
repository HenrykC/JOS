using System;
using System.Collections.Generic;

namespace Service.Jira.Models
{
    public class SprintReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Goal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Success { get; set; }
        public double Velocity { get; set; }
        public double VelocitySprintGoal { get; set; }
        public double Scope { get; set; }
        public IList<Issue> Issues{ get; set; }
    }
}
