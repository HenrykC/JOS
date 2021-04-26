using System;

namespace Service.Jira.Models
{
    public class SprintReport
    {
        public string Name { get; set; }
        public string Goal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Success { get; set; }
        public double Velocity { get; set; }
        public double VelocitySprintGoal { get; set; }
        public double Scope { get; set; }

    }
}
