using System;
using System.Collections.Generic;

namespace Service.Outlook.Models
{
    public class AppointmentDuration
    {
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class SprintAppointmentDuration
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; }
        public List<AppointmentDuration> AppointmentDurations { get; set; }
    }
}
