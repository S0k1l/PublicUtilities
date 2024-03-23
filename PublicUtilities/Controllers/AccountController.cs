using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.Services;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IPhotoService _photoService;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IAccountRepository accountRepository,
            IPhotoService photoService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountRepository = accountRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        //TODO: Check plase of residance is exist 
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

            var imgResult = await _photoService.AddPhotoAsync(model.Image);
            var imageUrl = imgResult.Url.ToString();

            var newUser = new AppUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Surname = model.Surname,
                Name = model.Name,
                Patronymic = model.Patronymic,
                ImageUrl = imageUrl,
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Користувача з такою електронною поштою не існує");
                return View(model);
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordCheck)
            {
                ModelState.AddModelError("Password", "Неправильно введений пароль");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                if (User.IsInRole(UserRoles.Admin)) { return RedirectToAction("AdminPanel", "Home"); }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddPlaceOfResidence()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaceOfResidence(AddPlaceOfResidenceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _accountRepository.GetUserByUserName(User.Identity.Name);

            if (user == null) 
            { 
                ModelState.AddModelError("", "Користувача не існує"); 
                return View(model);
            }

            var placeOfResidence = await _accountRepository.GetPlacesOfResidencer(model.Street, model.Building, model.Apartment);

            if (placeOfResidence == null)
            {
                ModelState.AddModelError("", "Такого місця проживання немає в базі даних");
                return View(model);
            }

            var userPlaceOfResidence = new UsersPlacesOfResidence
            {
                AppUser = user,
                AppUserId = user.Id,
                PlacesOfResidence = placeOfResidence,
                PlacesOfResidenceId = placeOfResidence.Id,
            };

            if (_accountRepository.AddPlaceOfResidence(userPlaceOfResidence))
            {
                return RedirectToAction("ManageAccount");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccount()
        {
            if (!ModelState.IsValid) return View();

            var user = await _accountRepository.GetUserByUserName(User.Identity.Name);

            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не існує");
                return View();
            }

            var userPlacesOfResidemce = await _accountRepository.GetUserPlacesOfResidencerById(user.Id);

            var model = new ManageAccountViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                Name = user.Name,
                Patronymic = user.Patronymic,
                PlacesOfResidenceViewModel = userPlacesOfResidemce,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageAccount(ManageAccountViewModel model)
        {
            if (!ModelState.IsValid) return View(ModelState);

            var user = await _accountRepository.GetUserByUserName(User.Identity.Name);

            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не існує");
                return View();
            }

            model.PlacesOfResidenceViewModel = await _accountRepository.GetUserPlacesOfResidencerById(user.Id);

            user.UserName = model.Email;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Surname = model.Surname;
            user.Name = model.Name;
            user.Patronymic = model.Patronymic;

            await _userManager.UpdateNormalizedUserNameAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (!result.Succeeded) { return BadRequest(model); }
            }
            if (_accountRepository.UpdateUserInfo(user)) { return View(model); }

            ModelState.AddModelError("", "Помилка при оновленні даних");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserPlaceOfResidence(int placeOfResidenceId)
        {
            var user = await _accountRepository.GetUserByUserName(User.Identity.Name);

            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не існує");
                return View(ModelState);
            }

            var userPlaceOfResidence = await _accountRepository.GetUserPlacesOfResidence(user.Id, placeOfResidenceId);
            
            if (userPlaceOfResidence == null)
            {
                ModelState.AddModelError("", "Користувач немає такого місця проживання");
                return View(ModelState);
            }

            if (_accountRepository.DeleteUserPlaceOfResidence(userPlaceOfResidence))
            {
                return RedirectToAction("ManageAccount");
            }

            return View(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
