using Global.Models;
using Global.Models.Outlook;
using Microsoft.EntityFrameworkCore;

namespace Service.Produktteam.Capacity
{
    public sealed class OutlookDbContext : DbContext
    {
        public OutlookDbContext(DbContextOptions<OutlookDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<DailyCapacity> PersonalCapacities { get; set; }
    }
}
