using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.Outlook
{
    public class DailyCapacity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Capacity { get; set; }
        public DateTime MeasureTimeStamp { get; set; } = DateTime.Now;
        public string UserName { get; set; }
    }
}
