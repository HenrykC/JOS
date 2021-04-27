using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public IssueRepository(IConnectionProfile connectionProfile, IMapper mapper)
        {
            _mapper = mapper;
            _jira = Atlassian.Jira.Jira.CreateRestClient(
                url: connectionProfile.Url,
                username: connectionProfile.UserName,
                password: Security.Cryption.Base64Decode(connectionProfile.Password));
        }

        public Issue GetIssue(string key)
        {
            var queryResult = JsonConvert.DeserializeObject<IssueDbModel>(
                _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET,
                        $"{_jiraPath}/issue/{key}")
                    .Result
                    .ToString());

            return _mapper.Map<Issue>(queryResult);
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
    }
}
