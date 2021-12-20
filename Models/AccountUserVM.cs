using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TennisWebApp8.Models
{
    public class AccountUserVM
    {        
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }
        public int Age { get; set; }        
        public bool MembershipPaid { get; set; }
    }
}
