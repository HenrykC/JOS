using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Jira.Models
{
    public class ConnectionProfile : IConnectionProfile
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
