using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Models.Outlook;

namespace Service.Outlook.Logic
{
    public interface IAppointmentLogic
    {
        Task<List<Models.Appointment>> GetAppointments(DateTime startDate, DateTime endDate);
        Task<List<DailyCapacity>> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail);
    }
}