using System;
using System.Collections.Generic;
using Global.Models.Outlook;

namespace Service.Produktteam.Capacity
{
    public interface IOutlookRepository
    {
        int Add(PersonalCapacity personalCapacity);
        int Add(List<DailyCapacity> capacity);

        DateTime LastCapacityMeasure();
        double GetCapacity(List<string> userNames, DateTime startDate, DateTime endDate);
        List<DailyCapacity> GetDailyCapacity(string userName, DateTime startDate, DateTime endDate);
    }
}