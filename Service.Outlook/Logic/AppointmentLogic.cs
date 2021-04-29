using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Service.Outlook.Models;
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

        public async Task<SprintAppointmentDuration> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail)
        {
            var appointments = await _appointmentRepository.GetAppointmentsAsync(startDate, endDate, userMail);
            var sprintDuration = new SprintAppointmentDuration()
            {
                AppointmentDurations = new List<AppointmentDuration>()
            };

            foreach (DateTime day in EachDay(startDate, endDate))
            { 
                var duration = new AppointmentDuration();
                
                duration.DateTime = day;
                duration.Duration = new TimeSpan(
                                            appointments.Where(w => w.Start.Date.Equals(day))
                                                .Sum(s => s.Duration.Ticks));

                if (duration.Duration > new TimeSpan(0, 8, 0, 0))
                {
                   // duration.Duration = new TimeSpan(0, 8, 0, 0);
                }
                sprintDuration.AppointmentDurations.Add(duration);
                sprintDuration.Duration += duration.Duration;
            }

            sprintDuration.StartDate = startDate;
            sprintDuration.EndDate = endDate;
            
            return sprintDuration;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
