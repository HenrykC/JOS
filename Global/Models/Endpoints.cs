using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models
{
    public static class Endpoints
    {
        public struct Outlook
        {
            public const string Base = "https://localhost:5000/";
            public const string Appointment = "https://localhost:5000//api/Appointment";
        }

        public struct Jira
        {
            public const string Base = "https://localhost:4000/";
            public const string Capacity = "https://localhost:4000/api/report/Capacity";
            public const string Report = "https://localhost:4000/api/report";
        }
    }
}
