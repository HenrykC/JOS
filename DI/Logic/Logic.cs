using DI.DataLayer;
using DI.Model;
using System.Collections.Generic;

namespace DI.LogicNs
{
    public class Logic : ILogic
    {
        private readonly IEFRepository eFRepository;

        public Logic(IEFRepository eFRepository)
        {
            this.eFRepository = eFRepository;
        }

        public int Add(Ticket ticket)
        {
            var result = eFRepository.Add(ticket);

            return result;
        }

        public List<Ticket> Get()
        {
            var result = eFRepository.Get();

            return result;
        }
    }
}
