using Adal.DbContexts;
using Adal.Utilities;
using Core.CoreClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Adal.Controllers
{
    public class LoginController : Controller
    {

        private readonly DatabaseContext _context;

        public LoginController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Users Model)
        {
            
            var users = await _context.Users.FirstOrDefaultAsync(c=>c.Email == Model.Email);

            if (users != null)
            {
                if (users.Password == Model.Password)
                {
                    return RedirectToAction("Index","Home");
                }
            }

            return View(Model);
        }


        public IActionResult Register()
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
        public async Task<IActionResult> Register(UsersDTO Model)
        {
            if (ModelState.IsValid)
            {
                var utilities = new UtilitiesClass<UsersDTO, Users>();
                var user = utilities.Map(Model);
				user.UserRoleId = 3;
				user.UserType = 0;
				_context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }

            var getCity = _context.City.ToList();

            foreach (var item in getCity)
            {
                Model.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            return View(Model);
        }


        public IActionResult LawyerRegister()
        {
            UsersDTO model = new UsersDTO();

            var getCity = _context.City.ToList();

            foreach (var item in getCity)
            {
                model.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            var lawyerTypes = _context.LawyerTypes.ToList();

            foreach (var item in lawyerTypes)
            {
                model.LawyerTypeList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            model.UserRoleId = 3;
            model.Active = true;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LawyerRegister(UsersDTO Model)
        {
            if (ModelState.IsValid)
            {
                var utilities = new UtilitiesClass<UsersDTO, Users>();
                var user = utilities.Map(Model);
                user.UserRoleId = 2;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }

            var getCity = _context.City.ToList();

            foreach (var item in getCity)
            {
                Model.CityList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            return View(Model);
        }


    }
}
