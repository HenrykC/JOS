using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Service.Outlook.Repository
{
    public interface IAppointmentRepository
    {
        Task<List<Models.Appointment>> GetAppointmentsAsync(DateTime startDate, DateTime endDate, string userMail = null);
    }
}