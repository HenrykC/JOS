using DI.Outlook.Models;
using System.Collections.Generic;

namespace DI.Outlook.Repository
{
    public interface IOutlookRepository
    {
        int Add(Appointment appointment);
        bool Exists(string appointmentId);
        List<Appointment> Get();
    }
}