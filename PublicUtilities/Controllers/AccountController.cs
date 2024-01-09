using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Data;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel model)
        {
            if(!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Користувач з тикою електроною почтою вже існує");
                return View(model);
            }

            if(model.PhoneNumber.Length != 10)
            {
                ModelState.AddModelError("PhoneNumber", "Номер телефону введений не правильно");
                return View(model);
            }

            var newUser = new AppUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Surname = model.Surname,
                Name = model.Name,
                Patronymic = model.Patronymic,
                UsersPlacesOfResidenceId = new List<UsersPlacesOfResidence>() {
                    new UsersPlacesOfResidence
                    {
                        PlacesOfResidence = new PlacesOfResidence
                        {
                            Apartment = model.Apartment,
                            House = model.Building,
                            Streets = new Streets{ Name = model.Street},
                        }
                    }
                }

            };
            var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);

            if (newUserResponse.Succeeded)
            {
                //await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                var result = await _signInManager.PasswordSignInAsync(newUser, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
