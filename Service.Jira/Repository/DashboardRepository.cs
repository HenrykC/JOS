using Global.Security;
using Service.Jira.Models;
using Service.Jira.Models.Profiles;

namespace Service.Jira.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly Atlassian.Jira.Jira _jira;
        private readonly string _jiraPath = "rest/dashboards/1.0";

        public DashboardRepository(IConnectionProfile connectionProfile)
        {
            _jira = Atlassian.Jira.Jira.CreateRestClient(
                url: connectionProfile.Url,
                username: connectionProfile.UserName,
                password: Cryption.Base64Decode(connectionProfile.Password));
        }

        public bool UpdateGadget(int dashboardId, int gadgetId, DashboardContent content)
        {

            var result = //JsonConvert.DeserializeObject<Board>(
                _jira.RestClient.ExecuteRequestAsync(RestSharp.Method.PUT, $"{_jiraPath}/{dashboardId}/gadget/{gadgetId}/prefs", content)
                    .Result;

            return true;
        }
    }
}
