using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Exceptions;
using Global.Models.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Service.Outlook.Models.Profiles;

namespace Service.Outlook.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IOutlookConnectionProfile _connectionProfile;
        private readonly IMapper _mapper;

        public AppointmentRepository(IOutlookConnectionProfile connectionProfile, IMapper mapper)
        {
            _connectionProfile = connectionProfile;
            _mapper = mapper;
        }

        public async Task<List<Models.Appointment>> GetAppointmentsAsync(DateTime startDate, DateTime endDate, string userMail = null)
        {
            List<Outlook.Models.Appointment> results = new List<Outlook.Models.Appointment>();
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            Stopwatch watch = new Stopwatch();
            //var results = new List<Appointment>();
            if (string.IsNullOrEmpty(_connectionProfile.UserName)) throw new ConfigurationException("Exchange username is not set in user.pwd");
            if (string.IsNullOrEmpty(_connectionProfile.Password)) throw new ConfigurationException("Exchange password is not set in user.pwd");
            if (string.IsNullOrEmpty(_connectionProfile.Domain)) throw new ConfigurationException("Exchange domain is not set in user.pwd");
            if (string.IsNullOrEmpty(_connectionProfile.Email)) throw new ConfigurationException("Exchange email is not set in user.pwd");

            service.Credentials = new System.Net.NetworkCredential(
                _connectionProfile.UserName,
                 Global.Security.Cryption.Base64Decode(_connectionProfile.Password),
                _connectionProfile.Domain);

            if (_connectionProfile.AutodiscoverUrl)
            {
                watch.Start();
                service.AutodiscoverUrl(_connectionProfile.Email, (discoverURL) => true);
                resetWatch(watch, "AutodiscoverUrl");
            }
            else
            {
                service.Url = new Uri(_connectionProfile.MailServerUrl);
            }

            const int NUM_APPTS = 1000;

            FolderId calender = WellKnownFolderName.Calendar;
            if (!string.IsNullOrEmpty(userMail))
            {
                calender = new FolderId(WellKnownFolderName.Calendar, userMail);
            }

            CalendarFolder calendar = await CalendarFolder.Bind(service, calender, new PropertySet());
            resetWatch(watch, "Connect Exchange");

            CalendarView cView = new CalendarView(startDate, endDate, NUM_APPTS);

            // Limit the properties returned to the appointment's subject, start time, and end time.
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End);
            FindItemsResults<Appointment> appointments = await calendar.FindAppointments(cView);
            resetWatch(watch, $"found {appointments.TotalCount} Appointments");

            var counter = 0;
            foreach (var appointment in appointments)
            {
                //await appointment.Load();
                //if (!outlookRepository.Exists(appointment.Id.UniqueId))
                {
                    //outlookRepository.Add(mapper.Map<Appointment>(appointment));
                    results.Add(_mapper.Map<Models.Appointment>(appointment));
                }
                counter++;
                Debug.WriteLine($"{counter}/{appointments.TotalCount}");
            }
            resetWatch(watch, $"mapped {appointments.TotalCount} Appointments");

            return results;
        }

        private void resetWatch(Stopwatch watch, string message)
        {
            watch.Stop();
            Debug.WriteLine($"{watch.ElapsedMilliseconds} ms: {message}");
            watch.Restart();
        }
    }
}
