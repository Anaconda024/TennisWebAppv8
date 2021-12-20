using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisWebApp8.Contracts;
using TennisWebApp8.Data;
using TennisWebApp8.Models;

namespace TennisWebApp8.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AccountUser> _userManager;


        public PlayersController(IUnitOfWork unit, IMapper mapper, ApplicationDbContext db, UserManager<AccountUser> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
        }

        // GET: PlayersController
        public ActionResult PlayerIndex()
        {
            var players = _userManager.GetUsersInRoleAsync("ClientPlayer").Result;
            var model = _mapper.Map<List<AccountUser>, List<AccountUserVM>>(players.ToList());
            return View(model);
        }



        // GET: PlayersController/Edit/5
        public ActionResult Edit(string id)
        {
            string playerid = id.ToString();
            var isExists = _userManager.FindByIdAsync(playerid).Result; ;
            if (isExists == null)
            {
                return NotFound();
            }

            var booking =  _userManager.FindByIdAsync(playerid).Result; 
            var model = _mapper.Map<AccountUser>(booking);
            return View(model);
        }

        // POST: PlayersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccountUser model)
        {
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                model.UserName = model.FirstName;
                var user = _userManager.FindByIdAsync(model.Id).Result;

                user.MembershipPaid = model.MembershipPaid;
                user.Age = model.Age;
                user.Level = model.Level;
                
                await _userManager.UpdateAsync(user);
                
                return RedirectToAction(nameof(PlayerIndex));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: PlayersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlayersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
