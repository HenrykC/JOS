using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Service.Jira.Models.Repository
{

    public class SprintQueryResult
    {
        public int MaxResults { get; set; }
        public int StartAt { get; set; }
        public bool IsLast { get; set; }
        public List<Sprint> Sprints { get; set; }
    }

    public class SprintIssueQueryResult
    {
        public int MaxResults { get; set; }
        public int StartAt { get; set; }
        public bool IsLast { get; set; }
        public List<IssueDbModel> Issues { get; set; }
    }
}
