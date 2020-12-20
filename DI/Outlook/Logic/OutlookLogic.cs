using AutoMapper;
using DI.Exceptions;
using DI.Outlook.Models;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointment = Microsoft.Exchange.WebServices.Data.Appointment;

namespace DI.Outlook.Logic
{
    public class OutlookLogic : IOutlookLogic
    {
        private readonly IMapper mapper;
        private readonly IOutlookConnectionProfile outlookConnectionProfile;


        public OutlookLogic(IMapper mapper, IOutlookConnectionProfile outlookConnectionProfile)
        {
            this.mapper = mapper;
            this.outlookConnectionProfile = outlookConnectionProfile;
        }

     
        public async Task<List<Outlook.Models.Appointment>> GetLastWeek()
        {
            List<Outlook.Models.Appointment> results = new List<Outlook.Models.Appointment>();
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            if (string.IsNullOrEmpty(outlookConnectionProfile.UserName)) throw new ConfigurationException("Outlook username is not set in appsettings");
            if (string.IsNullOrEmpty(outlookConnectionProfile.Password)) throw new ConfigurationException("Outlook password is not set in appsettings");
            if (string.IsNullOrEmpty(outlookConnectionProfile.Domain)) throw new ConfigurationException("Outlook domain is not set in appsettings");
            if (string.IsNullOrEmpty(outlookConnectionProfile.Email)) throw new ConfigurationException("Outlook email is not set in appsettings");

            service.Credentials = new WebCredentials("Henrykc", "", "HiScout.com");
           

            service.AutodiscoverUrl("Cwikowski@HiScout.com", (discoverURL) => true);


            if (service != null)
            {
                // Initialize values for the start and end times, and the number of appointments to retrieve.
                DateTime startDate = DateTime.Now;
                DateTime endDate = startDate.AddDays(7);
                const int NUM_APPTS = 100;
                // Initialize the calendar folder object with only the folder ID. 
                CalendarFolder calendar = await CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());
                // Set the start and end time and number of appointments to retrieve.
                CalendarView cView = new CalendarView(startDate, endDate, NUM_APPTS);
                // Limit the properties returned to the appointment's subject, start time, and end time.
                cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End);
                // Retrieve a collection of appointments by using the calendar view.
                FindItemsResults<Appointment> appointments = await calendar.FindAppointments(cView);
                Console.WriteLine("\nThe first " + NUM_APPTS + " appointments on your calendar from " + startDate.Date.ToShortDateString() +
                                  " to " + endDate.Date.ToShortDateString() + " are: \n");


                foreach (Appointment appointment in appointments)
                {
                    results.Add(mapper.Map<Models.Appointment>(appointment));
                }
            }


            return results;
        }
    }
}
