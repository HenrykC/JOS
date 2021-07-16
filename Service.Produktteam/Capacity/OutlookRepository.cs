using System;
using System.Collections.Generic;
using System.Linq;
using Global.Models.Outlook;
using Microsoft.EntityFrameworkCore;

namespace Service.Produktteam.Capacity
{
    public class OutlookRepository : IOutlookRepository
    {
        private readonly DbContextOptions<OutlookDbContext> options;

        public OutlookRepository(DbContextOptions<OutlookDbContext> options)
        {
            this.options = options;
        }

        public int Add(PersonalCapacity capacity)
        {
            using var dbContext = new OutlookDbContext(options);

            dbContext.Add(capacity);
            dbContext.SaveChanges();
            return capacity.Id;
        }

        public int Add(List<DailyCapacity> capacity)
        {
            using var dbContext = new OutlookDbContext(options);

            dbContext.AddRange(capacity);
            var added = dbContext.SaveChanges();
            return added;
        }

        public DateTime LastCapacityMeasure()
        {
            using var dbContext = new OutlookDbContext(options);
            return dbContext.PersonalCapacities.Max(m => m.MeasureTimeStamp);
        }
    }
}
