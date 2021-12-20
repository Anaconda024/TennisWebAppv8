using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TennisWebApp8.Models
{
    public class TimeslotVM
    {
        [Key]
        public int Id { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public bool Booked { get; set; }
    }

    public class DisplayTimeslotVM
    {
        public int TotalTimeslots { get; set; }
        public int TotalBooked { get; set; }
        public int TotalAvailable { get; set; }
        public List<TimeslotVM> Timeslots { get; set; }
    }
}
