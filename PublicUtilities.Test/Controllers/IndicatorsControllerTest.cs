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
    public class IndicatorsControllerTest
    {
        private readonly IIndicatorsRepository _indicatorsRepository;
        private readonly IndicatorsController _indicatorsController;

        public IndicatorsControllerTest()
        {
            //Dependencies
            _indicatorsRepository = A.Fake<IIndicatorsRepository>();

            //SUT
            _indicatorsController = new IndicatorsController(_indicatorsRepository);
        }

        [Fact]
        public async Task IndicatorsController_HttpGet_Index_ReturnsViewResult()
        {
            //Arrand
            var claims = new[] { new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _indicatorsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var model = A.Fake<ICollection<IndicatorsViewModel>>();


            A.CallTo(() => _indicatorsRepository.GetIndeicatorsByUserName(A<string>._)).Returns(model);

            //Act
            var result = await _indicatorsController.Index();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task IndicatorsController_HttpPost_Index_ReturnsRedirectToAction()
        {
            //Arrange
            int placesOfResidenceId = 000;
            string utilitiesName = "fakeUtilitiesName";
            string indicator = "fakeIndicator";
            string consumed = "123";
            var utilities = A.Fake<Utilities>();

            A.CallTo(() => _indicatorsRepository.GetUtilities(utilitiesName)).Returns(utilities);
            A.CallTo(() => _indicatorsRepository.AddIndicator(A<Indicators>._)).Returns(true);

            //Act
            var result = await _indicatorsController.Index(placesOfResidenceId, utilitiesName, indicator, consumed);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task IndicatorsController_IndicatorsList_ReturnsViewResult()
        {
            //Arrange
            var model = A.Fake<ICollection<IndicatorsListViewModel>>();
            A.CallTo(() => _indicatorsRepository.GetThoseWhoDontUploadIndicators()).Returns(model);

            //Act
            var result = await _indicatorsController.IndicatorsList();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
    }
}
