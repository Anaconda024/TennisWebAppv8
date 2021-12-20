using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TennisWebApp8.Data
{
    public class Dayslot
    {
        [Key]
        public int DayId { get; set; }
        public string Day { get; set; }
    }
}
