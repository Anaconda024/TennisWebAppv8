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
    public class TimeslotsController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AccountUser> _userManager;


        public TimeslotsController(IUnitOfWork unit, IMapper mapper, ApplicationDbContext db, UserManager<AccountUser> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
        }

        // GET: Timeslots
        public async Task<IActionResult> TimeslotIndex()
        {
            var timeslots = await _unit.Timeslots.FindAll();            
            var timeslotsModel = _mapper.Map<List<TimeslotVM>>(timeslots);
            var model = new DisplayTimeslotVM
            {
                TotalTimeslots = timeslotsModel.Count,
                TotalAvailable = timeslotsModel.Where(q => q.Booked == false).Count(),
                TotalBooked = timeslotsModel.Where(q => q.Booked == true).Count(),
                Timeslots = timeslotsModel
            };
            return View(model);
        }

        // GET: Timeslots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var isExists = await _unit.Timeslots.isExist(q => q.Id == id);
            if (isExists == false)
            {
                return NotFound();
            }

            var timeslots = await _unit.Timeslots
                .Find(q => q.Id == id);
            var model = _mapper.Map<TimeslotVM>(timeslots);

            return View(model);
        }
            // GET: Timeslots/Create
            public IActionResult Create()
        {
            ViewBag.Days = new SelectList(_db.Dayslots, "DayId", "Day");
            return View();
        }

        // POST: Timeslots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeslotVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var session = _mapper.Map<Timeslot>(model);
            await _unit.Timeslots.Create(session);
            await _unit.Save();

            return RedirectToAction(nameof(TimeslotIndex));
        }

        // GET: Timeslots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var isExists = await _unit.Timeslots.isExist(q => q.Id == id);
            if (isExists == false)
            {
                return NotFound();
            }

            var timeslots = await _unit.Timeslots.Find(q => q.Id == id);
            var model = _mapper.Map<Timeslot>(timeslots);
            return View(model);
        }

        // POST: Timeslots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Timeslot timeslot)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unit.Timeslots.Update(timeslot);
                    await _unit.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var isexisting = await _unit.Timeslots.isExist(q => q.Id == timeslot.Id);
                    if (!isexisting)
                    {return NotFound();}

                    else
                    {throw;}
                }
                return RedirectToAction(nameof(TimeslotIndex));
            }
            return View(timeslot);
        }

        // GET: Timeslots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var isExists = await _unit.Timeslots.isExist(q => q.Id == id);
            if (isExists == false)
            {
                return NotFound();
            }

            var timeslots = await _unit.Timeslots.Find(q => q.Id == id);
            var model = _mapper.Map<Timeslot>(timeslots);
            return View(model);
        }

        // POST: Timeslots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeslots = await _unit.Timeslots.Find(q => q.Id == id);            
            _unit.Timeslots.Delete(timeslots);
            await _unit.Save();

            return RedirectToAction(nameof(TimeslotIndex));
        }

        
    }
}
