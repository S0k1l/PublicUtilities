using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Controllers;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PublicUtilities.Test.Controllers
{
    public class PaymentControllerTest
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentController _paymentController;

        public PaymentControllerTest()
        {
            //Dependencies
            _paymentRepository = A.Fake<IPaymentRepository>();

            //STU
            _paymentController = new PaymentController(_paymentRepository);
        }

        [Fact]
        public async Task PaymentController_Water_ReturnsViewResult()
        {
            //Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _paymentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var model = A.Fake<ICollection<PaymentViewModel>>();

            A.CallTo(() => _paymentRepository.GetUserPaymentByUserName(A<string>._, A<string>._)).Returns(model);

            //Act
            var result = await _paymentController.Water();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task PaymentController_Gas_ReturnsViewResult()
        {
            //Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _paymentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var model = A.Fake<ICollection<PaymentViewModel>>();

            A.CallTo(() => _paymentRepository.GetUserPaymentByUserName(A<string>._, A<string>._)).Returns(model);

            //Act
            var result = await _paymentController.Gas();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task PaymentController_Electricity_ReturnsViewResult()
        {
            //Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _paymentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var model = A.Fake<ICollection<PaymentViewModel>>();

            A.CallTo(() => _paymentRepository.GetUserPaymentByUserName(A<string>._, A<string>._)).Returns(model);

            //Act
            var result = await _paymentController.Electricity();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void PaymentController_HttpGet_PaymentType_ReturnsViewResult()
        {
            //Arrange
            int id = 000;
            string placeOfResidence = "fakePlaceOfResidence";
            string price = "123";
            string date = "fakeDate";
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _paymentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            //Act
            var result = _paymentController.PaymentType(id, placeOfResidence, price, date);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task PaymentController_HttpPost_PaymentType_ReturnsRedirectToActionResult()
        {
            //Arrange
            int id = 000;
            var indicator = A.Fake<Indicators>();

            A.CallTo(() => _paymentRepository.GetUserPaymentById(id)).Returns(indicator);
            A.CallTo(() => _paymentRepository.PeymetntOperation(indicator)).Returns(true);

            //Act
            var result = await _paymentController.PaymentType(id);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Water");
        }

        [Fact]
        public async Task PaymentController_PaymentList_ReturnsViewResult()
        {
            //Assert
            var model = A.Fake<ICollection<PaymentListViewModel>>();

            A.CallTo(() => _paymentRepository.GetPaymentList()).Returns(model);

            //Act
            var result = await _paymentController.PaymentList();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
    }
}
