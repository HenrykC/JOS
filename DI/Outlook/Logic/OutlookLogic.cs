using AutoMapper;
using DI.Exceptions;
using DI.Outlook.Models;
using DI.Outlook.Repository;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Appointment = Microsoft.Exchange.WebServices.Data.Appointment;

namespace DI.Outlook.Logic
{
    public class OutlookLogic : IOutlookLogic
    {
        private readonly IMapper mapper;
        private readonly IOutlookConnectionProfile outlookConnectionProfile;
        private readonly IOutlookRepository outlookRepository;

        public OutlookLogic(IMapper mapper, IOutlookConnectionProfile outlookConnectionProfile, IOutlookRepository outlookRepository)
        {
            this.mapper = mapper;
            this.outlookConnectionProfile = outlookConnectionProfile;
            this.outlookRepository = outlookRepository;
        }

        public IList<Models.Appointment> GetAllAppointments()
        {
            return outlookRepository.Get();
        }

        public async Task<IList<Models.Appointment>> GetLastWeek()
        {
            List<Outlook.Models.Appointment> results = new List<Outlook.Models.Appointment>();
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            Stopwatch watch = new Stopwatch();
            if (string.IsNullOrEmpty(outlookConnectionProfile.UserName)) throw new ConfigurationException("Outlook username is not set in appsettings");
            if (string.IsNullOrEmpty(outlookConnectionProfile.Password)) throw new ConfigurationException("Outlook password is not set in appsettings");
            if (string.IsNullOrEmpty(outlookConnectionProfile.Domain)) throw new ConfigurationException("Outlook domain is not set in appsettings");
            if (string.IsNullOrEmpty(outlookConnectionProfile.Email)) throw new ConfigurationException("Outlook email is not set in appsettings");

            service.Credentials = new WebCredentials(outlookConnectionProfile.UserName, outlookConnectionProfile.Password, outlookConnectionProfile.Domain);


            watch.Start();
            service.AutodiscoverUrl(outlookConnectionProfile.Email, (discoverURL) => true);
            resetWatch(watch, "AutodiscoverUrl");

            if (service != null)
            {
                // Initialize values for the start and end times, and the number of appointments to retrieve.
                DateTime startDate = new DateTime(2020, 12, 1); //new DateTime(2020, 12, 01);
                DateTime endDate = new DateTime(2021, 01, 31); // new DateTime(2021, 1, 31);
                const int NUM_APPTS = 1000;
                // Initialize the calendar folder object with only the folder ID. 
                CalendarFolder calendar = await CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());
                resetWatch(watch, "Connect Exchange");
                // Set the start and end time and number of appointments to retrieve.
                CalendarView cView = new CalendarView(startDate, endDate, NUM_APPTS);
                // Limit the properties returned to the appointment's subject, start time, and end time.
                cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End);
                // Retrieve a collection of appointments by using the calendar view.
                FindItemsResults<Appointment> appointments = await calendar.FindAppointments(cView);
                resetWatch(watch, $"found {appointments.TotalCount} Appointments");

                int counter = 0;
                foreach (Appointment appointment in appointments)
                {

                    if (!outlookRepository.Exists(appointment.Id.UniqueId))
                    {
                        await appointment.Load();
                        outlookRepository.Add(mapper.Map<Models.Appointment>(appointment));
                        results.Add(mapper.Map<Models.Appointment>(appointment));
                    }
                    counter++;
                    Debug.WriteLine($"{counter}/{appointments.TotalCount}");
                }
                resetWatch(watch, $"mapped {appointments.TotalCount} Appointments");
            }


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
