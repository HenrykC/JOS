using Microsoft.EntityFrameworkCore;

namespace DI.Model
{
    public class JiraContext : DbContext
    {

        public JiraContext(DbContextOptions<JiraContext> options)
        : base(options)
        {

        }

        public DbSet<Ticket> Tickets { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source=jira.db");
    }
}
