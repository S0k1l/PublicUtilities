using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicUtilities.Controllers;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PublicUtilities.Test.Controllers
{
    public class HomeControllerTest
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly HomeController _homeController;

        public HomeControllerTest()
        {
            //Dependencies
            _logger = A.Fake<ILogger<HomeController>>();
            _homeRepository = A.Fake<IHomeRepository>();

            //SUT
            _homeController = new HomeController(_logger, _homeRepository);
        }

        [Fact]
        public async Task HomeController_Index_ReturnsViewResult()
        {
            //Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var model = A.Fake<ICollection<News>>();

            A.CallTo(() => _homeRepository.GetTop4News()).Returns(model);

            //Act
            var result = await _homeController.Index();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
    }
}
