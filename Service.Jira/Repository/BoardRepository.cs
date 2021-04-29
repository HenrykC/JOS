using Global.Security;
using Newtonsoft.Json;
using Service.Jira.Models;
using Service.Jira.Models.Profiles;

namespace Service.Jira.Repository
{
    public class BoardRepository : IBoardRepository
    {
        private readonly Atlassian.Jira.Jira _jira;
        private readonly string _jiraPath = "rest/agile/1.0/";

        public BoardRepository(IConnectionProfile connectionProfile)
        {
            _jira = Atlassian.Jira.Jira.CreateRestClient(
               url: connectionProfile.Url,
               username: connectionProfile.UserName,
               password: Cryption.Base64Decode(connectionProfile.Password));
        }

        public Board GetBoardById(int id)
        {
            var board = JsonConvert.DeserializeObject<Board>(
                            _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.GET, $"{_jiraPath}board/{id}")
                            .Result
                            .ToString());

            return board;
        }
    }
}
