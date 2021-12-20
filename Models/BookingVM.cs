using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TennisWebApp8.Models
{
    public class BookingVM
    {
        [Key]
        public int Id { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Length { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public string Coach { get; set; }
        public string CoachId { get; set; }
        public int TimeslotId { get; set; }
        public string AccountId { get; set; }
    }
}
