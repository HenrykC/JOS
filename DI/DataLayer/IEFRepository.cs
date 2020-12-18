using DI.Model;
using System.Collections.Generic;

namespace DI.DataLayer
{
    public interface IEFRepository
    {
        List<Ticket> Get();
        int Add(Ticket ticket);
    }
}