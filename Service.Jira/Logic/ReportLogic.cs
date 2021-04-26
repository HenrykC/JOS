using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.Jira.Models;
using Service.Jira.Repository;

namespace Service.Jira.Logic
{
    public class ReportLogic : IReportLogic
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueRepository _issueRepository;

        public ReportLogic(ISprintRepository sprintRepository, IIssueRepository issueRepository)
        {
            _sprintRepository = sprintRepository;
            _issueRepository = issueRepository;
        }

        public List<SprintReport> GetReport(int boardId)
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
                    i.Resolutiondate != null && i.Resolutiondate.Value.CompareTo(sprint.EndDate) <= 0);

                if (success)
                    sucessSprint++;
                else
                    failedSprint++;

                foreach (var issue4Estimation in issues
                    .Where(i => i.Resolutiondate != null)
                    .Where(i => i.Resolutiondate.Value.CompareTo(sprint.EndDate) <= 0)
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

                sprintReport.Add(
                    new SprintReport()
                    {
                        Name = sprint.Name,
                        Goal = sprint.Goal,
                        StartDate = sprint.StartDate,
                        EndDate = sprint.EndDate,
                        Success = success,
                        VelocitySprintGoal = sprintVelocity,
                        Velocity = issues.Where(i => i.Resolutiondate != null && i.Resolutiondate.Value.CompareTo(sprint.EndDate) <= 0)
                                            .Where(i => i.Status.Name.ToLower().Equals("done"))
                                            .Sum(s => s.Estimation ?? 0.0),
                        Scope = issues.Sum(s => s.Estimation ?? 0.0),
                    });
            }

            return sprintReport;
        }
    }
}
