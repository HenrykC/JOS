using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Outlook;
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

            List<TimeSpan> meetingIntervals = Enumerable.Range(0, 20) // 07:00- 11:45
                .Select(x => x * 15)
                .Select(x => startWorking + TimeSpan.FromMinutes(x))
                .ToList();

            startWorking = new TimeSpan(13, 00, 0);
            meetingIntervals.AddRange(Enumerable.Range(0, 12) // 13:00 - 15:45
                .Select(x => x * 15 * 60)
                .Select(x => startWorking + TimeSpan.FromSeconds(x)));
            
            var capacity = new List<DailyCapacity>();
            
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
                                               a.FreeBusyStatus == LegacyFreeBusyStatus.WorkingElsewhere)))
                    {

                        if (dailyCapacity.Capacity < 0.1) continue;

                        dailyCapacity.Capacity -= 0.25;
                    }
                }
                capacity.Add(dailyCapacity);
            }

            return capacity;
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
