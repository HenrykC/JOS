using System;
using System.Collections.Generic;

namespace Global.Models.Outlook
{
    public class PersonalCapacity
    {
        public int Id { get; set; }
        public List<DailyCapacity> Capacity { get; set; }
        public double SprintCapacity { get; set; }
        public double HandsOnKeyboard { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
