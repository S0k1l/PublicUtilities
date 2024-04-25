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
    public class NotificationControllerTest
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationController _notificationController;

        public NotificationControllerTest()
        {
            //Dependencies
            _notificationRepository = A.Fake<INotificationRepository>();

            //SUT
            _notificationController = new NotificationController(_notificationRepository);
        }

        [Fact]
        public async Task NotificationController_Index_ReturnsViewResult()
        {
            //Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _notificationController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var userPlacesOfResidence = A.Fake<ICollection<PlacesOfResidence>>();
            var notifications = A.Fake<ICollection<Notifications>>();

            A.CallTo(() => _notificationRepository.GetUserPlacesOfResidences(A<string>._)).Returns(userPlacesOfResidence);
            A.CallTo(() => _notificationRepository.GetStreetNotifications(userPlacesOfResidence)).Returns(notifications);
            A.CallTo(() => _notificationRepository.GetHouseNotifications(userPlacesOfResidence)).Returns(notifications);
            A.CallTo(() => _notificationRepository.GetDirectNotifications(userPlacesOfResidence)).Returns(notifications);
            A.CallTo(() => _notificationRepository.GetGlobalNotification()).Returns(notifications);

            //Act
            var result = await _notificationController.Index();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task NotificationController_HttpGet_Create_ReturnsViewResult()
        {
            //Arrange
            int placesOfResidenceId = 111;
            string header = "fakeHeader";
            string text = "fakeHeader";
            var por = A.Fake<PlacesOfResidence>();
            var street = A.Fake<Streets>();
            por.Streets = street;

            A.CallTo(() => _notificationRepository.GetUserPlaceOfResidences(placesOfResidenceId)).Returns(por);

            //Act
            var result = await _notificationController.Create(placesOfResidenceId, header, text);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void NotificationController_HttpPost_Create_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = A.Fake<CreateNotificationViewModel>();

            A.CallTo(() => _notificationRepository.Add(A<Notifications>._)).Returns(true);

            //Act
            var result = _notificationController.Create(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Edit");
        }

        [Fact]
        public async Task NotificationController_HttpGet_Edit_ReturnsViewResult()
        {
            //Arrange
            int id = 000;
            var notifiction = A.Fake<Notifications>();

            A.CallTo(() => _notificationRepository.GetNotification(id)).Returns(notifiction);

            //Act
            var result = await _notificationController.Edit(id);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task NotificationController_HttpPost_Edit_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = A.Fake<EditNotificationsViewModel>();
            var notification = A.Fake<Notifications>();

            A.CallTo(() => _notificationRepository.GetNotification(model.Id)).Returns(notification);
            A.CallTo(() => _notificationRepository.Update(notification)).Returns(true);

            //Act
            var result = await _notificationController.Edit(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Edit");
        }

        [Fact]
        public async Task NotificationController_Delete_ReturnsRedirectToActionResult()
        {
            //Arrange
            int id = 000;
            var notification = A.Fake<Notifications>();

            A.CallTo(() => _notificationRepository.GetNotification(id)).Returns(notification);
            A.CallTo(() => _notificationRepository.Remove(notification)).Returns(true);

            //Act
            var result = await _notificationController.Delete(id);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Index");
        }
    }
}
