using System;
using Newtonsoft.Json;

namespace Service.Jira.Models
{
    public class Sprint
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("completeDate")]
        public DateTime CompleteDate { get; set; }

        [JsonProperty("originBoardId")]
        public int OriginBoardId { get; set; }

        [JsonProperty("goal")]
        public string Goal { get; set; }
    }
}
