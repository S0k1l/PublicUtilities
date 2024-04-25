using CloudinaryDotNet.Actions;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Controllers;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.Repository;
using PublicUtilities.Services;
using PublicUtilities.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PublicUtilities.Test.Controllers
{
    public class AccountControllerTest
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IPhotoService _photoService;
        private readonly AccountController _accountController;
        public AccountControllerTest()
        {
            //Dependencies
            _userManager = A.Fake<UserManager<AppUser>>();
            _signInManager = A.Fake<SignInManager<AppUser>>();
            _accountRepository = A.Fake<IAccountRepository>();
            _photoService = A.Fake<IPhotoService>();

            //SUT
            _accountController = new AccountController(_userManager,_signInManager, _accountRepository, _photoService);
        }

        [Fact]
        public async Task AccountController_Registration_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = new RegisterViewModel { PhoneNumber = "1234567890", Image = A.Fake<IFormFile>() };
            var imgResult = A.Fake<ImageUploadResult>();
            var a = new Url("asd");

            A.CallTo(() => _userManager.FindByEmailAsync(A<string>._)).Returns((AppUser)null);
            A.CallTo(() => _photoService.AddPhotoAsync(A<IFormFile>._))
                .Returns(Task.FromResult(new ImageUploadResult { Url = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ") }));
            A.CallTo(() => _userManager.CreateAsync(A<AppUser>._, A<string>._)).Returns(IdentityResult.Success);
            A.CallTo(() => _signInManager.PasswordSignInAsync(A<AppUser>._, A<string>._, false, false))
                .Returns(Microsoft.AspNetCore.Identity.SignInResult.Success);


            //Act
            var result = await _accountController.Registration(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Index");
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task AccountController_Login_ReturnsRedirectToActionResult()
        {

            //Arrange
            var model = A.Fake<LoginViewModel>();
            var user = A.Fake<AppUser>();
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            A.CallTo(() => _userManager.FindByEmailAsync(A<string>._)).Returns(user);
            A.CallTo(() => _userManager.CheckPasswordAsync(A<AppUser>._, A<string>._)).Returns(true);
            A.CallTo(() => _signInManager.PasswordSignInAsync(A<AppUser>._, A<string>._, false, false))
                .Returns(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await _accountController.Login(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Index");
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task AccountController_AddPlaceOfResidence_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = A.Fake<AddPlaceOfResidenceViewModel>();
            var user = A.Fake<AppUser>();
            var por = A.Fake<PlacesOfResidence>();
            var claims = new[] { new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };


            A.CallTo(() => _accountRepository.GetUserByUserName(A<string>._)).Returns(user);
            A.CallTo(() => _accountRepository.GetPlacesOfResidencer(A<string>._, A<string>._, A<string>._)).Returns(por);
            A.CallTo(() => _accountRepository.AddPlaceOfResidence(A<UsersPlacesOfResidence>._)).Returns(true);
            //Act
            var result = await _accountController.AddPlaceOfResidence(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("ManageAccount");
        }

        [Fact]
        public async Task AccountController_HttpGet_ManageAccount_ReturnsViewResult()
        {
            //Act
            var claims = new[] { new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var user = A.Fake<AppUser>();
            var colection = new List<PlacesOfResidenceViewModel>();

            A.CallTo(() => _accountRepository.GetUserPlacesOfResidencerById(user.Id)).Returns(colection);

            //Act
            var result = await _accountController.ManageAccount();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task AccountController_HttpPost_ManageAccount_ReturnsViewResult()
        {
            //Arrenge
            var model = A.Fake<ManageAccountViewModel>();
            var claims = new[] { new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var user = A.Fake<AppUser>();
            var colection = new List<PlacesOfResidenceViewModel>();
            var code = "fakeCode";

            A.CallTo(() => _accountRepository.GetUserByUserName(A<string>._)).Returns(user);
            A.CallTo(() => _accountRepository.GetUserPlacesOfResidencerById(user.Id)).Returns(colection);
            A.CallTo(() => _userManager.UpdateNormalizedUserNameAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            A.CallTo(() => _userManager.UpdateNormalizedEmailAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            A.CallTo(() => _userManager.GeneratePasswordResetTokenAsync(user)).Returns(code);
            A.CallTo(() => _userManager.ResetPasswordAsync(user, code, model.Password)).Returns(IdentityResult.Success);
            A.CallTo(() => _accountRepository.UpdateUserInfo(user)).Returns(true);

            //Act
            var result = await _accountController.ManageAccount(model);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task AccountController_DeleteUserPlaceOfResidence_ReturnsRedirectToActionResult()
        {
            //Arrenge
            int fakeId = 000;
            var claims = new[] { new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var user = A.Fake<AppUser>();
            var userPlaceOfResidence = A.Fake<UsersPlacesOfResidence>();


            A.CallTo(() => _accountRepository.GetUserByUserName(A<string>._)).Returns(user);
            A.CallTo(() => _accountRepository.GetUserPlacesOfResidence(user.Id, fakeId)).Returns(userPlaceOfResidence);
            A.CallTo(() => _accountRepository.DeleteUserPlaceOfResidence(userPlaceOfResidence)).Returns(true);


            //Act
            var result = await _accountController.DeleteUserPlaceOfResidence(fakeId);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("ManageAccount");
        }
    }
}
