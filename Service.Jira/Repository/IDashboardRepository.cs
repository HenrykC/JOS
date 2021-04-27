using Service.Jira.Models;

namespace Service.Jira.Repository
{
    public interface IDashboardRepository
    {
        bool UpdateGadget(int dashboardId, int gadgetId, DashboardContent content);
    }
}