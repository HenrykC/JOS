using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Outlook.Models;

namespace Service.Outlook.Logic
{
    public interface IAppointmentLogic
    {
        Task<List<Models.Appointment>> GetAppointments(DateTime startDate, DateTime endDate);
        Task<SprintAppointmentDuration> GetAppointmentDuration(DateTime startDate, DateTime endDate, string userMail);
    }
}