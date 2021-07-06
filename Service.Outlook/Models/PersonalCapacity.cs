using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Service.Outlook.Models
{
    public class PersonalCapacity
    {
        public Dictionary<DateTime, double> Capacity { get; set; }
        public double SprintCapacity { get; set; }
        public double HandsOnKeyboard { get; set; }
        public string UserName { get; set; }

        //public double Daily { get; set; }
        //public double Education { get; set; }
        //public double Planning { get; set; }

        //public double Refinement { get; set; }
        //public double ReviewRetro { get; set; }
        //public double TeamReview { get; set; }


        //public double IK { get; set; }
        //public double ItTeam { get; set; }
        //public double QA { get; set; }
        //public double Support { get; set; }
        //public double SlackTime { get; set; }
        //public double Canvas { get; set; }
        //public double Away { get; set; }

        //public List<Appointment> MiscAppointments { get; set; } = new List<Appointment>();

        //public double RepeatingScrumHours => Daily + Planning + Refinement + ReviewRetro + TeamReview;

        //public double NonRepeatingScrumHours => Education + IK + ItTeam + QA + Support + SlackTime + Canvas;

        //public double MiscellaneousHours
        //{
        //    get { return MiscAppointments?.Sum(s => s.Duration.TotalHours) ?? 0; }
        //}

        //public double wohandsOnKeyboard => RepeatingScrumHours + NonRepeatingScrumHours + MiscellaneousHours + Away;
    }
}
