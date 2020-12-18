using DI.Model;
using System.Collections.Generic;

namespace DI.LogicNs
{
    public interface ILogic
    {
        List<Ticket> Get();
        int Add(Ticket ticket);
    }
}