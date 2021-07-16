using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Models;
using Global.Models.Outlook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Service.Outlook.Repository;

namespace Service.Outlook.Logic
{
    public class AppointmentLogic : IAppointmentLogic
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentLogic(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Models.Appointment>> GetAppointments(DateTime startDate, DateTime endDate)
        {
            var result = await _appointmentRepository.GetAppointmentsAsync(startDate, endDate);

            return result;
        }

        public async Task<List<DailyCapacity>> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail)
        {
            var appointments = await _appointmentRepository.GetAppointmentsAsync(startDate, endDate, userMail);
            var startWorking = new TimeSpan(7, 00, 0);

            List<TimeSpan> meetingIntervals = Enumerable.Range(0, 20) // till 07:00- 11:45
                .Select(x => x * 15)
                .Select(x => startWorking + TimeSpan.FromMinutes(x))
                .ToList();

            startWorking = new TimeSpan(13, 00, 0);
            meetingIntervals.AddRange(Enumerable.Range(0, 20) // till 13:00 - 17:45
                .Select(x => x * 15 * 60)
                .Select(x => startWorking + TimeSpan.FromSeconds(x)));

            //Dictionary<DateTime, double> capacity = new Dictionary<DateTime, double>();
            List<DailyCapacity> capacity = new List<DailyCapacity>();


            foreach (DateTime day in EachDay(startDate, endDate))
            {
                var dailyCapacity = new DailyCapacity()
                {
                    Date = day,
                    Capacity = 8,
                    UserName = userMail
                };

                foreach (TimeSpan span in meetingIntervals)
                {
                    if (appointments.Any(a => a.Start <= day.Add(span) && a.End >= day.Add(span).AddMinutes(15) &&
                                              (a.FreeBusyStatus == LegacyFreeBusyStatus.Busy ||
                                               a.FreeBusyStatus == LegacyFreeBusyStatus.OOF ||
                                               a.FreeBusyStatus == LegacyFreeBusyStatus.WorkingElsewhere ||
                                               a.FreeBusyStatus == LegacyFreeBusyStatus.Tentative)))
                    {

                        if (dailyCapacity.Capacity < 0.1) continue;

                        dailyCapacity.Capacity -= 0.25;
                    }
                }
                capacity.Add(dailyCapacity);
            }

            return capacity;
        }

        private void SetDetails()
        {
            //for (var i = appointments.Count - 1; i >= 0; i--)
            //{
            //    var removeAppointment = true;

            //    switch (appointments[i].Subject.ToLower())
            //    {
            //        case "daily":
            //            personalCapacity.Daily += appointments[i].Duration.TotalHours;
            //            break;
            //        case "scrum ausbildung":
            //            personalCapacity.Education += appointments[i].Duration.TotalHours;
            //            break;
            //        case "po-ausbildung":
            //            personalCapacity.Education += appointments[i].Duration.TotalHours;
            //            break;
            //        case "planning":
            //            personalCapacity.Planning += appointments[i].Duration.TotalHours;
            //            break;
            //        case "refinement":
            //            personalCapacity.Refinement += appointments[i].Duration.TotalHours;
            //            break;
            //        case "review + retro":
            //            personalCapacity.ReviewRetro += appointments[i].Duration.TotalHours;
            //            break;
            //        case "review produktteam":
            //            personalCapacity.TeamReview += appointments[i].Duration.TotalHours;
            //            break;
            //        case "review modulteam":
            //            personalCapacity.TeamReview += appointments[i].Duration.TotalHours;
            //            break;
            //        case "review maps-team":
            //            personalCapacity.TeamReview += appointments[i].Duration.TotalHours;
            //            break;
            //        case "interne koordination (ik)":
            //            personalCapacity.IK += appointments[i].Duration.TotalHours;
            //            break;
            //        case "slacktime":
            //            personalCapacity.SlackTime += appointments[i].Duration.TotalHours;
            //            break;

            //        case string str when str.Contains("ausbildung"):
            //            personalCapacity.Education += appointments[i].Duration.TotalHours;
            //            break;
            //        default:
            //            removeAppointment = false;
            //            break;
            //    }

            //    if (removeAppointment)
            //        appointments.RemoveAt(i);

            //}

            //personalCapacity.MiscAppointments = appointments;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                if ((day.DayOfWeek >= DayOfWeek.Monday) && (day.DayOfWeek <= DayOfWeek.Friday))
                    yield return day;
            }
        }
    }
}
