using DI.Outlook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.Outlook.Repository
{
    public class OutlookRepository : IOutlookRepository
    {
        private readonly DbContextOptions<OutlookDbContext> options;

        public OutlookRepository(DbContextOptions<OutlookDbContext> options)
        {
            this.options = options;
        }

        public int Add(Appointment appointment)
        {
            using var dbContext = new OutlookDbContext(options);

            var existingApps = dbContext.Appointments.AsNoTracking()
                                                    .Where(app => app.AppointmentId.Equals(appointment.AppointmentId))
                                                    .ToList();

            bool alreadyExists = false;
            foreach (Appointment app in existingApps)
            {
                app.Id = 0;
                if (app.GetHashCode() == appointment.GetHashCode())
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                dbContext.Add(appointment);
                dbContext.SaveChanges();
            }

            return appointment.Id;
        }

        public bool Exists(string appointmentId)
        {
            using var dbContext = new OutlookDbContext(options);

            return dbContext.Appointments.Where(app => app.AppointmentId.Equals(appointmentId)).FirstOrDefault() != null;
        }

        public List<Appointment> Get()
        {
            using var dbContext = new OutlookDbContext(options);

            return dbContext.Appointments.ToList();
        }

    }
}
