using DI.Exceptions;
using DI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DI.DataLayer
{
    public class EFRepository : IEFRepository
    {
        private readonly DbContextOptions<JiraContext> options;

        public EFRepository(DbContextOptions<JiraContext> options)
        {
            this.options = options;
        }

        public int Add(Ticket ticket)
        {
            using var dbContext = new JiraContext(options);

            dbContext.Add(ticket);
            dbContext.SaveChanges();

            return ticket.Id;
        }

        public List<Ticket> Get()
        {
            using var dbContext = new JiraContext(options);

            return dbContext.Tickets.ToList();
        }
    }
}
