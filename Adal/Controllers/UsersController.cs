using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Adal.DbContexts;
using Core.CoreClass;
using Adal.Models;
using Adal.Utilities;

namespace Adal.Controllers
{
    public class UsersController : Controller
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }



		// GET: Users
		public async Task<IActionResult> Index()
		{
			var getUsers = await _context.Users.Where(x=>x.UserRoleId > 1).ToListAsync();

			var usersDTOList = new List<UsersDTO>();

			if (getUsers != null)
			{
				var getCity = _context.City.ToList();
				var utilities = new UtilitiesClass<Users, UsersDTO>();

				// Call CreateMapWithAutoProperties to automatically include all properties in the mapping
				//utilities.CreateMapWithAutoProperties();

				foreach (var item in getUsers)
				{
					var usersDTO = utilities.Map(item);

					if (item.UserRoleId == 1)
					{
						usersDTO.UserRoleName = "Clint";
					}
					if (item.UserRoleId == 2)
					{
						usersDTO.UserRoleName = "Lawyer";
					}
					if (item.UserRoleId == 3)
					{
						usersDTO.UserRoleName = "Client";
					}
					if (getCity.FirstOrDefault(z => z.Id == item.CityId) != null)
					{
						usersDTO.CityName = getCity.FirstOrDefault(z => z.Id == item.CityId).Name;
					}
					usersDTOList.Add(usersDTO);
				}
				return View(usersDTOList);
			}

			return View(new List<UsersDTO>());
		}



		#region Client Working

		public async Task<IActionResult> ClientList()
		{
			var getUsers = await _context.Users.Where(x => x.UserRoleId == 3).ToListAsync();

			var usersDTOList = new List<UsersDTO>();

			if (getUsers != null)
			{
				var getCity = _context.City.ToList();
				var utilities = new UtilitiesClass<Users, UsersDTO>();

				// Call CreateMapWithAutoProperties to automatically include all properties in the mapping
				//utilities.CreateMapWithAutoProperties();

				foreach (var item in getUsers)
				{
					var usersDTO = utilities.Map(item);

					if (item.UserRoleId == 1)
					{
						usersDTO.UserRoleName = "Clint";
					}
					if (getCity.FirstOrDefault(z => z.Id == item.CityId) != null)
					{
						usersDTO.CityName = getCity.FirstOrDefault(z => z.Id == item.CityId).Name;
					}
					usersDTOList.Add(usersDTO);
				}
				return View(usersDTOList);
			}

			return View(new List<UsersDTO>());
		}

		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }
        
		public IActionResult Create()
        {
            UsersDTO model = new UsersDTO();

            var getCity = _context.City.ToList();

			foreach (var item in getCity)
			{
				model.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
			}

			model.UserRoleId = 3;
			model.Active = true;

			return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersDTO users)
        {
            if (ModelState.IsValid)
            {
                var utilities = new UtilitiesClass<UsersDTO, Users>();
                var user = utilities.Map(users);
				user.UserType = 0;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ClientList));
            }

            var getCity = _context.City.ToList();
        
			foreach (var item in getCity)
			{
				users.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
			}

			return View(users);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);

			if (users != null)
            {
				var getCity = _context.City.ToList();

				var utilities = new UtilitiesClass<Users, UsersDTO>();
				var usersDTO = utilities.Map(users);

                foreach (var item in getCity)
                {
                    usersDTO.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name, Selected = item.Id == users.CityId });
				}
				usersDTO.LawyerTypeList.Add(new SelectListItem { Value = "1", Text = "Standard" });
				usersDTO.LawyerTypeList.Add(new SelectListItem { Value = "2", Text = "Premium" });


				return View(usersDTO);

            }
			return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsersDTO users)
        {
            var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
                return NotFound();
            }

            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					//var utilities = new UtilitiesClass<UsersDTO, Users>();
					//var user = utilities.Map(users);

					user.Address = users.Address;
					user.CNIC = users.CNIC;
					user.CityId = users.CityId;
					user.Contact = users.Contact;
					user.FirstName = users.FirstName;
					user.LastName = users.LastName;
					user.DateOfBirth = users.DateOfBirth;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				if (users.UserRoleId == 2)
				{
					return RedirectToAction(nameof(LawyerList));
				}
				return RedirectToAction(nameof(ClientList));

			}
			return View(users);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var users = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users.FirstName + " "+ users.LastName);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DatabaseContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);

			var isClient = false;

            if (users != null)
            {
				isClient = users.UserRoleId == 3;
                _context.Users.Remove(users);
            }
			await _context.SaveChangesAsync();

			if (isClient)
			{
				return RedirectToAction(nameof(ClientList));
			}
			return RedirectToAction(nameof(LawyerList));
        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

		#endregion

		#region Lawyer 

		public async Task<IActionResult> LawyerList()
		{
			var getUsers = await _context.Users.Where(x => x.UserRoleId == 2).ToListAsync();

			var usersDTOList = new List<UsersDTO>();

			if (getUsers != null)
			{
				var getCity = _context.City.ToList();
				var utilities = new UtilitiesClass<Users, UsersDTO>();

				foreach (var item in getUsers)
				{
					var usersDTO = utilities.Map(item);

					if (item.UserRoleId == 2)
					{
						usersDTO.UserRoleName = "Lawyer";
					}
					if (getCity.FirstOrDefault(z => z.Id == item.CityId) != null)
					{
						usersDTO.CityName = getCity.FirstOrDefault(z => z.Id == item.CityId).Name;
					}
					usersDTOList.Add(usersDTO);
				}
				return View(usersDTOList);
			}
			return View(new List<UsersDTO>());
		}

		public IActionResult Register()
        {
            UsersDTO model = new UsersDTO();

            var getCity = _context.City.ToList();

			foreach (var item in getCity)
			{
				model.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name});
			}
			model.LawyerTypeList.Add(new SelectListItem { Value = "1", Text = "Standard" });
			model.LawyerTypeList.Add(new SelectListItem { Value = "2", Text = "Premium" });
			model.UserRoleId = 2;
			model.Active = false;

			return View(model);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(UsersDTO users)
		{
			if (ModelState.IsValid)
			{
				var utilities = new UtilitiesClass<UsersDTO, Users>();
				var user = utilities.Map(users);

				_context.Add(user);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			var getCity = _context.City.ToList();

			foreach (var item in getCity)
			{
				users.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
			}
			return View(users);
		}

		[HttpPost]
		public async Task<IActionResult> DeActive(int id)
		{
			if (_context.Users == null)
			{
				return Json("Error");
			}
			var users = await _context.Users.FindAsync(id);

			if (users != null)
			{
				users.Active = false;
				_context.Users.Update(users);
				await _context.SaveChangesAsync();
			}
			return Json("Success");
		}

		#endregion
	}
}
