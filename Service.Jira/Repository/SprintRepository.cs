using System.Collections.Generic;
using System.Linq;
using Global.Security;
using Service.Jira.Models;
using Newtonsoft.Json;
using Service.Jira.Models.Profiles;
using Service.Jira.Models.Repository;

namespace Service.Jira.Repository
{
    public class SprintRepository : ISprintRepository
    {
        private readonly Atlassian.Jira.Jira _jira;
        private readonly string _jiraPath = "rest/agile/1.0";

        public SprintRepository(IConnectionProfile connectionProfile)
        {
            _jira = Atlassian.Jira.Jira.CreateRestClient(
                url: connectionProfile.Url,
                username: connectionProfile.UserName,
                password: Cryption.Base64Decode(connectionProfile.Password));
        }
        public Sprint GetSprint(int id)
        {
            var sprint = JsonConvert.DeserializeObject<Sprint>(
                _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET, $"{_jiraPath}/sprint/{id}")
                    .Result
                    .ToString());

            return sprint;
        }

        public IList<Sprint> GetAllSprints(int boardId)
        {
            var result = new List<Sprint>();
            var startQueryAt = 0;
            var maxResults = 50;
            SprintQueryResult queryResult;

            do
            {
                queryResult = JsonConvert.DeserializeObject<SprintQueryResult>(
                    _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                            $"{_jiraPath}/board/{boardId}/sprint?startAt={startQueryAt}&maxResults={maxResults}")
                        .Result
                        .ToString());

                result.AddRange(queryResult.Sprints.Where(s => s.CompleteDate.Year > 2020).ToList());
                startQueryAt += maxResults;

            } while (!queryResult.IsLast && queryResult.Sprints.Count > 0);

            return result;
        }
    }
}
