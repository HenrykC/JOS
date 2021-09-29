using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.Jira.Models;
using Service.Jira.Models.Profiles;
using Service.Jira.Repository;

namespace Service.Jira.Logic
{
    public class ReportLogic : IReportLogic
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IDashboardProfile _dashboardProfile;

        public ReportLogic(ISprintRepository sprintRepository,
                            IIssueRepository issueRepository,
                            IDashboardRepository dashboardRepository,
                            IDashboardProfile dashboardProfile
                            )
        {
            _sprintRepository = sprintRepository;
            _issueRepository = issueRepository;
            _dashboardRepository = dashboardRepository;
            _dashboardProfile = dashboardProfile;
        }

        public List<SprintReport> GenerateVelocity(int boardId)
        {
            var sprintReport = new List<SprintReport>();

            var fullVelocity = 0.0;
            var sucessVelocity = 0.0;
            var failedVelocity = 0.0;
            var sucessSprint = 0;
            var failedSprint = 0;

            var sprintList = _sprintRepository.GetAllSprints(boardId);

            foreach (var sprint in sprintList)
            {
                var sprintVelocity = 0.0;

                var issues = _issueRepository.GetAllIssuesBySprintId(sprint.Id);
                var sprintGoal = issues.Where(i => i.Priority?.Name.Equals("Must") ?? false).ToList();

                var success = sprintGoal.Count > 0 && sprintGoal.All(i =>
                    i.Resolutiondate != null && i.Resolutiondate.Value.CompareTo(sprint.CompleteDate) <= 0);

                if (success)
                    sucessSprint++;
                else
                    failedSprint++;

                foreach (var issue4Estimation in issues
                    .Where(i => i.Resolutiondate != null)
                    .Where(i => i.Resolutiondate.Value.CompareTo(sprint.CompleteDate) <= 0)
                    .Where(i => i.Estimation.HasValue)
                )
                {
                    var value = issue4Estimation.Estimation ?? 0.0;
                    sprintVelocity += value;
                    fullVelocity += value;
                    if (success)
                        sucessVelocity += value;
                    else
                        failedVelocity += value;
                }

                if (sprint.Name.Contains("41"))
                {

                }

                sprintReport.Add(
                    new SprintReport()
                    {
                        Id = sprint.Id,
                        Name = sprint.Name,
                        Goal = sprint.Goal,
                        StartDate = sprint.StartDate,
                        EndDate = sprint.CompleteDate,
                        Success = success,
                        VelocitySprintGoal = sprintVelocity,
                        Velocity = issues.Where(i => i.Resolutiondate != null && i.Resolutiondate.Value.CompareTo(sprint.CompleteDate) <= 0)
                                            .Where(i => i.Status.Name.ToLower().Equals("done"))
                                            .Sum(s => s.Estimation ?? 0.0),
                        Scope = issues.Sum(s => s.Estimation ?? 0.0),
                        Issues = issues
                    });
            }

            foreach (var gadgetProfile in _dashboardProfile.GadgetProfiles)
            {
                var content = new DashboardContent()
                {
                    Html = GenerateReportHtml(sprintReport.Where(w => w.Name.ToLower().Contains(gadgetProfile.SprintNameFilter.ToLower())).ToList()),
                    IsConfigured = true,
                    Title = gadgetProfile.GadgetName
                };

                _dashboardRepository.UpdateGadget(_dashboardProfile.DashBoardId, gadgetProfile.GadgetId, content);
            }

            return sprintReport;
        }

        public bool GenerateCapacity(int gadgetId, string htmlText)
        {
            var content = new DashboardContent()
            {
                Html = htmlText,
                IsConfigured = true,
                Title = "Capacity"
            };

            return _dashboardRepository.UpdateGadget(_dashboardProfile.DashBoardId, gadgetId, content);
        }

        public SprintReport GetSprintReport(int sprintId)
        {
            var sprintVelocity = 0.0;

            var sprint = _sprintRepository.GetSprint(sprintId);
            var issues = _issueRepository.GetAllIssuesBySprintId(sprint.Id);
            var sprintGoal = issues.Where(i => i.Priority?.Name.Equals("Must") ?? false).ToList();

            var success = sprintGoal.Count > 0 && sprintGoal.All(i =>
                i.Resolutiondate != null && i.Resolutiondate.Value.CompareTo(sprint.CompleteDate) <= 0);


            sprintVelocity = issues
                                .Where(i => i.Resolutiondate != null)
                                .Where(i => i.Resolutiondate.Value.CompareTo(sprint.CompleteDate) <= 0)
                                .Where(i => i.Estimation.HasValue)
                                .Sum(s => s.Estimation ?? 0.0);

            var sprintReport = new SprintReport()
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Goal = sprint.Goal,
                StartDate = sprint.StartDate,
                EndDate = sprint.CompleteDate,
                Success = success,
                VelocitySprintGoal = sprintVelocity,
                Velocity = issues.Where(i => i.Resolutiondate != null && i.Resolutiondate.Value.CompareTo(sprint.CompleteDate) <= 0)
                    .Where(i => i.Status.Name.ToLower().Equals("done"))
                    .Sum(s => s.Estimation ?? 0.0),
                Scope = issues.Sum(s => s.Estimation ?? 0.0),
                Issues = issues
            };

            return sprintReport;
        }

        private string GenerateReportHtml(IEnumerable<SprintReport> sprints)
        {
            var html = string.Empty;

            var xDescription = 10;
            var yDescription = 50;
            var yBar = 35;

            var xSprintGoal = 160;

            foreach (var sprint in sprints)
            {
                var sprintGoalIssues = string.Join(" OR Key = ",
                    sprint.Issues.Where(i => i.Priority?.Name.Equals("Must") ?? false)
                        .Select(s => s.Key));

                var otherIssues = string.Join(" OR Key = ",
                    sprint.Issues.Where(i => !i.Priority?.Name.Equals("Must") ?? true)
                        .Select(s => s.Key));

                var success = sprint.Success ? "✓" : "X";
                var sucessColor = "#33cc33"; // sprint.Success ? "#33cc33" : "#B4472A";

                html +=
                    $"  <a href=\"{_dashboardProfile.JiraServer}/issues/?jql=project = PT AND Sprint = {sprint.Id} \" target=\"_blank\">" +
                    $"      <text x=\"{xDescription}\" y=\"{yDescription}\" font-size=\"12\" font-family=\"Arial\" fill=\"#404040\">{sprint.Name}({success})</text> \n" +
                    $"  </a>\n" +

                    $"  <a href=\"{_dashboardProfile.JiraServer}/issues/?jql=project = PT AND (Key = {sprintGoalIssues}) \" target=\"_blank\">" +
                    $"      <rect x = \"{xSprintGoal}\" y = \"{yBar}\" width = \"{50.0 * sprint.Velocity}\" height = \"20\" rx = \"3\" ry = \"3\" fill = \"{sucessColor}\" ></rect> \n" +
                    $"  </a>\n" +

                    $"  <a href=\"{_dashboardProfile.JiraServer}/issues/?jql=project = PT AND (Key = {otherIssues}) \" target=\"_blank\">" +
                    $"      <rect x = \"{50.0 * sprint.Velocity + xSprintGoal}\" y = \"{yBar}\" width = \"{50.0 * (sprint.Scope - sprint.Velocity)}\" height = \"20\" rx = \"3\" ry = \"3\" fill = \"#2A7BB4\" ></rect> \n" +
                    $"  </a>\n\n";

                yDescription += 30;
                yBar += 30;

            }

            html = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=ISO-8859-1\"> \n" +
                   "<title>SVG Diagramm</title> \n" +
                   " \n" +
                   " \n" +
                   "<style type=\"text/css\"> \n" +
                   "	#statSvg{background-image:url('svg-rasterlinie.jpeg');} \n" +
                   "	#statSvg rect{opacity: 0.9;} \n" +
                   "	#statSvg rect:hover{opacity: 0.6;} \n" +
                   "</style> \n" +
                   $"<svg id=\"statSvg\" xmlns=\"http://www.w3.org/2000/svg\" width=\"800\" height=\"{yBar + 50}\"> \n" +
                   html +
                   $"	<line x1=\"{xSprintGoal - 1}\" y1=\"10\" x2=\"{xSprintGoal - 1}\" y2=\"{yBar}\" stroke-width=\"2\" stroke=\"#808080\"></line> \n" +
                   $"	<line x1=\"{5 * 50 + xSprintGoal - 1}\" y1=\"10\" x2=\"{5 * 50 + xSprintGoal - 1}\" y2=\"{yBar}\" stroke-width=\"1\" stroke=\"#808080\"></line> \n" +
                   $"	<line x1=\"{10 * 50 + xSprintGoal - 1}\" y1=\"10\" x2=\"{10 * 50 + xSprintGoal - 1}\" y2=\"{yBar}\" stroke-width=\"1\" stroke=\"#808080\"></line> \n" +

                   "</svg> \n";

            return html;
        }
    }
}
