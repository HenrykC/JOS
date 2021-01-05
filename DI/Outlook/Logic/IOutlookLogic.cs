using DI.Outlook.Models;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DI.Outlook.Logic
{
    public interface IOutlookLogic
    {
        Task<IList<Appointment>> GetLastWeek();
        IList<Appointment> GetAllAppointments();
    }
}