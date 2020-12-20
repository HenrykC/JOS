using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.Outlook.Models
{
    public class Appointment
    {

        public int Id { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public String Organizer { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; }

        public AppointmentType AppointmentType { get; }

        public bool IsRecurring { get; }
        public bool IsCancelled { get; }
        public bool IsMeeting { get; }
        public bool IsAllDayEvent { get; set; }

    }
}
