using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Controllers;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicUtilities.Test.Controllers
{
    public class GarbageRemovalControllerTest
    {
        private readonly IGarbageRemovalRepository _garbageRemovalRepository;
        private readonly GarbageRemovalController _garbageRemovalController;

        public GarbageRemovalControllerTest()
        {
            //Dependencies
            _garbageRemovalRepository = A.Fake<IGarbageRemovalRepository>();

            //SUT
            _garbageRemovalController = new GarbageRemovalController(_garbageRemovalRepository);
        }

        [Fact]
        public async Task GarbageRemovalController_Index_ReturnsViewResult()
        {
            //Arrange
            var model = A.Fake<GarbageRemoval>();

            A.CallTo(() => _garbageRemovalRepository.GetGarbageRemovalAsync()).Returns(model);

            //Act
            var result = await _garbageRemovalController.Index();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task GarbageRemovalController_HttpGet_Edit_ReturnsViewResult()
        {
            //Arrange
            var model = A.Fake<GarbageRemoval>();

            A.CallTo(() => _garbageRemovalRepository.GetGarbageRemovalAsync()).Returns(model);

            //Act
            var result = await _garbageRemovalController.Edit();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task GarbageRemovalController_HttpPost_Edit_ReturnsViewResult()
        {
            //Arrange
            var model = A.Fake<GarbageRemoval>();
            var oldModel = A.Fake<GarbageRemoval>();

            A.CallTo(() => _garbageRemovalRepository.GetGarbageRemovalAsync()).Returns(oldModel);
            A.CallTo(() => _garbageRemovalRepository.Update(model)).Returns(true);

            //Act
            var result = await _garbageRemovalController.Edit(model);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
    }
}
