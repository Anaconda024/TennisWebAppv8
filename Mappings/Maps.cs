using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisWebApp8.Data;
using TennisWebApp8.Models;

namespace TennisWebApp8.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Booking, BookingVM>().ReverseMap();
            CreateMap<Timeslot, TimeslotVM>().ReverseMap();
            CreateMap<Timeslot, DisplayTimeslotVM>().ReverseMap();
            CreateMap<AccountUser, AccountUserVM>().ReverseMap();
            CreateMap<Interact, InteractVM>().ReverseMap();
        }
    }
}
