using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.Outlook.Models
{
    public class OutlookConnectionProfile : IOutlookConnectionProfile
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
    }
}
