using CloudinaryDotNet.Actions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Controllers;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.Services;
using PublicUtilities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PublicUtilities.Test.Controllers
{
    public class NewsControllerTest
    {

        private readonly INewsRepository _newsRepository;
        private readonly IPhotoService _photoService;
        private readonly NewsController _newsController;

        public NewsControllerTest()
        {
            //Dependencies
            _newsRepository = A.Fake<INewsRepository>();
            _photoService = A.Fake<IPhotoService>();

            //SUT
            _newsController = new NewsController(_newsRepository, _photoService);
        }

        [Fact]
        public async Task NewsController_Index_ReturnsViewResult()
        {
            //Arrange
            var model = A.Fake<NewsViewModel>();
            int page = 000;

            A.CallTo(() => _newsRepository.GetAllNews(page)).Returns(model);

            //Act
            var result = await _newsController.Index(page);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task NewsController_Details_ReturnsViewResult()
        {
            //Arrange
            int id = 000;
            var model = A.Fake<News>();

            A.CallTo(() => _newsRepository.GetNewsById(id)).Returns(model);

            //Act
            var result = await _newsController.Details(id);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task NewsController_Create_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = A.Fake<CreateNewsViewModel>();

            A.CallTo(() => _newsRepository.Add(A<News>._)).Returns(true);

            //Act
            var result = await _newsController.Create(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Details");
        }

        [Fact]
        public async Task NewsController_HttpGet_Edit_ReturnsViewResult()
        {
            //Arrange
            int id = 000;
            var news = A.Fake<News>();

            A.CallTo(() => _newsRepository.GetNewsById(id)).Returns(news);

            //Act
            var result = await _newsController.Edit(id);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task NewsController_HttpPost_Edit_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = A.Fake<EditNewsViewModel>();
            var news = A.Fake<News>();

            A.CallTo(() => _newsRepository.GetNewsById(model.Id)).Returns(news);
            A.CallTo(() => _newsRepository.Update(news)).Returns(true);

            //Act
            var result = await _newsController.Edit(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Details");
        }

        [Fact]
        public async Task NewsController_Delete_ReturnsRedirectToActionResult()
        {
            //Arrange
            int id = 000;
            var news = A.Fake<News>();
            var deletionResult = new DeletionResult { Result = "ok" };

            A.CallTo(() => _newsRepository.GetNewsById(id)).Returns(news);
            A.CallTo(() => _photoService.DeletePhotoAsync(news.ImageUrl)).Returns(deletionResult);
            A.CallTo(() => _newsRepository.Delete(news)).Returns(true);

            //Act
            var result = await _newsController.Delete(id);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("Index");

        }
    }
}
