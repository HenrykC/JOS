using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Models.Jira;
using Global.Security;
using Newtonsoft.Json;
using Service.Jira.Models;
using Service.Jira.Models.Profiles;
using Service.Jira.Models.Repository;

namespace Service.Jira.Repository
{
    public class IssueRepository : IIssueRepository
    {
        private readonly IMapper _mapper;
        private readonly Atlassian.Jira.Jira _jira;
        private readonly string _jiraPath = "rest/agile/1.0";
        private readonly string _jiraPathV2 = "rest/api/2";

        public IssueRepository(IConnectionProfile connectionProfile, IMapper mapper)
        {
            _mapper = mapper;
            _jira = Atlassian.Jira.Jira.CreateRestClient(
                url: connectionProfile.Url,
                username: connectionProfile.UserName,
                password: Cryption.Base64Decode(connectionProfile.Password));
        }

        public Issue GetIssue(string key)
        {
            var queryResult = JsonConvert.DeserializeObject<IssueDbModel>(
                _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                        $"{_jiraPath}/issue/{key}?expand=")
                    .Result
                    .ToString());

            var res = _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                    $"{_jiraPathV2}/issue/{key}/changelog")
                .Result.ToString();

            var result = _mapper.Map<Issue>(queryResult);
            var history =
                result.Changelog.Histories.Where(w => w.Items.Any(s => s.Field.Equals(ChangeReason.StoryPoints)));

            return result;
        }

        public IList<Issue> GetAllIssuesBySprintId(int sprintId)
        {
            var result = new List<Issue>();
            var startQueryAt = 0;
            const int maxResults = 50;
            SprintIssueQueryResult queryResult;

            do
            {
                queryResult = JsonConvert.DeserializeObject<SprintIssueQueryResult>(
                _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                        $"{_jiraPath}/sprint/{sprintId}/issue?startAt={startQueryAt}&maxResults={maxResults}")
                    .Result
                    .ToString());

                result.AddRange(queryResult.Issues.Select(s => _mapper.Map<Issue>(s)));
                startQueryAt += maxResults;

            } while (!queryResult.IsLast && queryResult.Issues.Count > 0);

            return result;
        }

        public IList<Issue> GetIssuesByJql(string jqlQuery)
        {
            var result = new List<Issue>();
            var startQueryAt = 0;
            const int maxResults = 50;
            SprintIssueQueryResult queryResult;

            do
            {
                queryResult = JsonConvert.DeserializeObject<SprintIssueQueryResult>(
                    _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                            $"{_jiraPathV2}/search?jql={jqlQuery}&startAt={startQueryAt}&maxResults={maxResults}")
                        .Result
                        .ToString());

                result.AddRange(queryResult.Issues.Select(s => _mapper.Map<Issue>(s)));
                startQueryAt += maxResults;

            } while (!queryResult.IsLast && queryResult.Issues.Count > 0);

            result.ForEach(f =>
            {
                f.Epic = new Epic()
                {
                    Name = "",
                    Key = ""
                };

                if (f.Fields.Customfield_10100 == null) return;

                f.Epic = GetEpic(f.Fields.Customfield_10100);

            });

            return result;
        }


        private Epic GetEpic(string key)
        {
            var queryResult = JsonConvert.DeserializeObject<Epic>(
                _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                        $"{_jiraPath}/epic/{key}")
                    .Result
                    .ToString());

            return queryResult;
        }
    }
}
