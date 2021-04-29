using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Outlook.Models
{
    public class Appointment
    {

        public int Id { get; set; }
        public string AppointmentId { get; set; }
        public String Subject { get; set; }
        public string Organizer { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }

        public AppointmentType AppointmentType { get; set; }

        public bool IsRecurring { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsMeeting { get; set; }
        public bool IsAllDayEvent { get; set; }

    }
}
