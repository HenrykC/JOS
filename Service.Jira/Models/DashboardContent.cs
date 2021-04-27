using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Service.Jira.Models
{
   

    public class DashboardContent
    {
        [JsonProperty("up_isConfigured")]
        public bool IsConfigured { get; set; }

        [JsonProperty("up_title")]
        public string Title { get; set; }

        [JsonProperty("up_html")]
        public string Html { get; set; }
    }
}
