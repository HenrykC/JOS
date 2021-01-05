using DI.Outlook.Models;
using Microsoft.EntityFrameworkCore;

namespace DI.Outlook.Repository
{
    public class OutlookDbContext : DbContext
    {
        public OutlookDbContext(DbContextOptions<OutlookDbContext> options)
            : base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }

    }

}
