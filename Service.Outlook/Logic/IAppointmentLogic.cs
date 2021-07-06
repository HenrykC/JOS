using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Models;
using Service.Outlook.Models;

namespace Service.Outlook.Logic
{
    public interface IAppointmentLogic
    {
        Task<List<Models.Appointment>> GetAppointments(DateTime startDate, DateTime endDate);
        Task<PersonalCapacity> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail);
    }
}