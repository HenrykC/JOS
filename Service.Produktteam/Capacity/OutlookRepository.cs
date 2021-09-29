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

        public double GetCapacity(List<string> userNames, DateTime startDate, DateTime endDate)
        {
            using var dbContext = new OutlookDbContext(options);
            var lastMeasure = dbContext.PersonalCapacities.Max(m => m.MeasureTimeStamp);
            lastMeasure = lastMeasure.AddSeconds(-60);
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var check = dbContext.PersonalCapacities
                .Where(w => w.MeasureTimeStamp.CompareTo(lastMeasure) >= 0)
                .Where(u => userNames.Contains(u.UserName))
                .Where(d => d.Date.CompareTo(startDate) >= 0 && d.Date.CompareTo(endDate) < 0)
                .ToList();


            var result = dbContext.PersonalCapacities
                .Where(w => w.MeasureTimeStamp.CompareTo(lastMeasure) >= 0)
                .Where(u => userNames.Contains(u.UserName))
                .Where(d => d.Date.CompareTo(startDate) >= 0 && d.Date.CompareTo(endDate) < 0)
                .Sum(s => s.Capacity);

            return result;
        }

        public List<DailyCapacity> GetDailyCapacity(string userName, DateTime startDate, DateTime endDate)
        {
            using var dbContext = new OutlookDbContext(options);
            var lastMeasure = dbContext.PersonalCapacities.Max(m => m.MeasureTimeStamp);
            lastMeasure = lastMeasure.AddSeconds(-60);
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = dbContext.PersonalCapacities
                .Where(w => w.MeasureTimeStamp.CompareTo(lastMeasure) >= 0)
                .Where(u => userName.ToLower().Equals(u.UserName.ToLower()))
                .Where(d => d.Date.CompareTo(startDate) >= 0 && d.Date.CompareTo(endDate) < 0)
                .OrderBy(o => o.Date)
                .ToList();

            return result;
        }
    }
}
