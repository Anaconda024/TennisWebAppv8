using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TennisWebApp8.Data
{
    public class AccountUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }
        public int Age { get; set; }
        public DateTime DateJoined { get; set; }
        public bool MembershipPaid { get; set; }
    }
    
}
