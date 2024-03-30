using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employee);
            var model = new List<EmployeeViewModel>();


            foreach (var user in users)
            {
                var claim = await _userManager.GetClaimsAsync(user);
                model.Add(new EmployeeViewModel
                {
                    Id = user.Id,
                    Surname = user.Surname,
                    Name = user.Name,
                    Patronymic = user.Patronymic,
                    Departement = claim.First().Value.ToString(),
                });
            }

            return View(model);
        }

        [HttpGet] 
        public async Task<IActionResult> Detalis(string Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var model = new EmployeeDetalisViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                Name = user.Name,
                Patronymic = user.Patronymic,
                Departement = userClaims.First().Value.ToString(),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            var model = new EmployeeEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                Name = user.Name,
                Patronymic = user.Patronymic,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (!ModelState.IsValid) { return View(); }

            var user = await _userRepository.GetUserByIdAsync(model.Id);

            if(user == null) { return View(); }

            var claim = ClaimStore.claimsList.FirstOrDefault(cl => cl.Type == model.Departement);

            if (claim == null) { return View(model); }

            var userClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.ReplaceClaimAsync(user, userClaims.First(), claim);


            if (model.Password != null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, model.Password);
            }

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Surname = model.Surname;
            user.Name = model.Name;
            user.Patronymic = model.Patronymic;

            if (_userRepository.Update(user))
            {
                return RedirectToAction("Index");

            }

            ModelState.AddModelError("", "Щось пішло не так при збережені");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if(user == null) { return RedirectToAction("Index"); }

            _userRepository.Delete(user);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Користувач з тикою електроною почтою вже існує");
                return View(model);
            }

            if (model.PhoneNumber.Length != 10)
            {
                ModelState.AddModelError("PhoneNumber", "Номер телефону введений не правильно");
                return View(model);
            }
            var claim = ClaimStore.claimsList.FirstOrDefault(cl => cl.Type == model.Departement);

            if (claim == null) { return View(model); }

            var newUser = new AppUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Surname = model.Surname,
                Name = model.Name,
                Patronymic = model.Patronymic,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Employee);
                await _userManager.AddClaimAsync(newUser, claim);
            }
            return View(model);
        }

    }
}
