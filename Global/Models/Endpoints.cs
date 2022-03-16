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
        }

        public struct Jira
        {
            public const string Base = "https://localhost:6000/";
            public const string Capacity = "https://localhost:6000/api/report/Capacity";
        }

        
    }
}
