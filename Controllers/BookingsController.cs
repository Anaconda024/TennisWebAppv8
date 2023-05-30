using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TennisWebApp8.Contracts;
using TennisWebApp8.Data;
using TennisWebApp8.Models;

namespace TennisWebApp8.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AccountUser> _userManager;


        public BookingsController(IUnitOfWork unit, IMapper mapper, ApplicationDbContext db, UserManager<AccountUser> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
        }

        // GET: Bookings
        public async Task<IActionResult> BookingIndex()
        {
            var bookings = await _unit.Bookings.FindAll();
            var model = _mapper.Map<List<Booking>, List<BookingVM>>(bookings.ToList());
            return View(model);
        }

        public async Task<IActionResult> ViewBooking()
        {
            var player = _userManager.GetUserAsync(User).Result;
            var bookings = await _unit.Bookings.FindAll(q => q.AccountId == player.Id);
            var model = _mapper.Map<List<Booking>, List<BookingVM>>(bookings.ToList());
            return View(model);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var isExists = await _unit.Bookings.isExist(q => q.Id == id);
            if (isExists == false)
            {
                return NotFound();
            }

            var booking = await _unit.Bookings
                .Find(q => q.Id == id);
            var model = _mapper.Map<Booking>(booking);

            return View(model);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewBag.Days = new SelectList(_db.Dayslots, "DayId", "Day");
            var coachlist = _userManager.GetUsersInRoleAsync("Coach").Result;
            ViewBag.Coachs = new SelectList(coachlist, "Id", "FirstName");
            return View();
        }

        public JsonResult LoadTimes(string ddlid)
        {
            var id = int.Parse(ddlid);
            var day = _unit.Dayslots.Find(q => q.DayId == id).Result;
            var times = _unit.Timeslots.FindAll(q => q.Day == day.Day && q.Booked == false).Result;
            var obgtime = new SelectList(times, "Id", "Time");
            return Json(obgtime);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingVM model)
        {
            try
            {
                var id = int.Parse(model.Day);
                var day = await _unit.Dayslots.Find(q => q.DayId == id);
                model.Day = day.Day;
                var player = _userManager.GetUserAsync(User).Result;
                if (player != null)
                {                    
                    model.FirstName = player.FirstName;
                    model.LastName = player.LastName;
                    model.AccountId = player.Id;
                }


                //var coach = _db.Users.Where(q => q.Id == model.CoachId);
                var coach = _userManager.FindByIdAsync(model.CoachId).Result;
                model.Coach = coach.UserName;

                if (model.Length == 30)
                {
                    var timeslot = _unit.Timeslots.Find(i => i.Day == model.Day && i.Time == model.Time).Result;
                    timeslot.Booked = true;
                    model.TimeslotId = timeslot.Id;
                    _unit.Timeslots.Update(timeslot);
                }
                else if (model.Length == 60)
                {
                    var timeslot = _unit.Timeslots.Find(i => i.Day == model.Day && i.Time == model.Time).Result;
                    var tsId = timeslot.Id + 1;

                    var timeslot2 = _unit.Timeslots.Find(q => q.Id == tsId).Result;
                    timeslot.Booked = true;
                    timeslot2.Booked = true;

                    _unit.Timeslots.Update(timeslot);
                    _unit.Timeslots.Update(timeslot2);

                }
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var session = _mapper.Map<Booking>(model);
                await _unit.Bookings.Create(session);
                await _unit.Save();

                // Send Email to coach and player
                //await _email.SenDEmailAsync("admin@tennis.com", "New Booking",
                    //$"Please review booking. <a href='UrlOfApp/{model.FirstName}'>Click here</a>");

                return RedirectToAction(nameof(BookingIndex));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return RedirectToAction(nameof(BookingIndex));
            }
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Days = new SelectList(_db.Dayslots, "DayId", "Day");
            var coachlist = _userManager.GetUsersInRoleAsync("Coach").Result;
            ViewBag.Coachs = new SelectList(coachlist, "Id", "FirstName");
            
            var isExists = await _unit.Bookings.isExist(q => q.Id == id);
            if (isExists == false)
            {
                return NotFound();
            }

            var booking = await _unit.Bookings.Find(q => q.Id == id);
            var model = _mapper.Map<Booking>(booking);
            return View(model);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var coach = _userManager.FindByIdAsync(booking.CoachId).Result;
                    booking.Coach = coach.UserName;
                    _unit.Bookings.Update(booking);
                    await _unit.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var isexisting = await _unit.Bookings.isExist(q => q.Id == booking.Id);
                    if (!isexisting)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BookingIndex));
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _unit.Bookings
                .Find(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        
        {
            
            try
            {
                var booking = _unit.Bookings.Find(q => q.Id == id).Result;
                if (booking == null)
                {
                    return NotFound();
                }
                _unit.Bookings.Delete(booking);

                if (booking.Length == 30)
                {
                    var timeslot = _unit.Timeslots.Find(i => i.Day == booking.Day && i.Time == booking.Time).Result;
                    timeslot.Booked = false;
                    _unit.Timeslots.Update(timeslot);
                }
                else if (booking.Length == 60)
                {
                    var timeslot = _unit.Timeslots.Find(i => i.Day == booking.Day && i.Time == booking.Time).Result;
                    var tsId = timeslot.Id + 1;

                    var timeslot2 = _unit.Timeslots.Find(q => q.Id == tsId).Result;
                    timeslot.Booked = false;
                    timeslot2.Booked = false;

                    _unit.Timeslots.Update(timeslot);
                    _unit.Timeslots.Update(timeslot2);

                }
                await _unit.Save();
                return RedirectToAction(nameof(BookingIndex));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return RedirectToAction(nameof(BookingIndex));
            }
        }

        //private bool BookingExists(int id)
        //{
        //return _unit.Bookings.Any(e => e.Id == id);
        //}





    }
}
