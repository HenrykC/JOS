using System;
using System.Collections.Generic;

namespace Global.Models
{
    public class PersonalAppointments
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; }
        public List<AppointmentDurationPerDay> AppointmentDurations { get; set; }
    }
}