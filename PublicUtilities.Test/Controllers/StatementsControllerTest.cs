using CloudinaryDotNet.Actions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Controllers;
using PublicUtilities.Data;
using PublicUtilities.Data.Enum;
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
    public class StatementsControllerTest
    {
        private readonly IStatementsRepository _statementsRepository;
        private readonly IPhotoService _photoService;
        private readonly StatementsController _statementsController;

        public StatementsControllerTest()
        {
            //Dependencies
            _statementsRepository = A.Fake<IStatementsRepository>();
            _photoService = A.Fake<IPhotoService>();

            //SUT
            _statementsController = new StatementsController(_statementsRepository, _photoService);
        }

        [Fact]
        public async Task StatementsController_MyStatements_ReturnsViewResult()
        {
            //Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _statementsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var statements = A.Fake<ICollection<MyStatements>>();

            A.CallTo(() => _statementsRepository.GetUserStatementByUserName(A<string>._)).Returns(statements);
            A.CallTo(() => _statementsRepository.GetOtherUserStatementByUserName(A<string>._)).Returns(statements);

            //Act
            var result = await _statementsController.MyStatements();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task StatementsController_StatementDetails_ReturnsViewResult()
        {
            //Arrange
            int id = 000;
            var statement = A.Fake<StatementDetailsViewModel>();

            A.CallTo(() => _statementsRepository.GetStatementDetailsById(id)).Returns(statement);

            //Act
            var result =await _statementsController.StatementDetails(id);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task StatementsController_HttpGet_SignStatement_ReturnsViewResult()
        {
            //Arrange
            int id = 000;
            var statement = A.Fake<StatementDetailsViewModel>();

            A.CallTo(() => _statementsRepository.GetStatementDetailsById(id)).Returns(statement);

            //Act
            var result = await _statementsController.SignStatement(id);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task StatementsController_HttpPost_SignStatement_ReturnsRedirectToActionResult()
        {
            //Arrange
            int id = 000;
            var statement = A.Fake<Statements>();
            var user = A.Fake<AppUser>();

            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _statementsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            A.CallTo(() => _statementsRepository.GetStatementByStatementId(id)).Returns(statement);
            A.CallTo(() => _statementsRepository.AddSignatureTostatement(statement)).Returns(true);
            A.CallTo(() => _statementsRepository.GetUserByUserName(A<string>._)).Returns(user);
            A.CallTo(() => _statementsRepository.AddStatement(A<UsersStatements>._)).Returns(true);

            //Act
            var result = await _statementsController.SignStatementPost(id);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("MyStatements");
        }

        [Fact]
        public async Task StatementsController_StatementsTypeList_ReturnsViewResult()
        {
            //Arrenge
            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _statementsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var model = A.Fake<ICollection<StatementsTypeListViewModel>>();

            A.CallTo(() => _statementsRepository.getAllStatements()).Returns(model);

            //Act
            var result = await _statementsController.StatementsTypeList();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task StatementsController_HttpGet_CreateStatement_ReturnsViewResult()
        {
            //Arrange
            int id = 000;

            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _statementsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var statementType = A.Fake<StatementsTypes>();

            A.CallTo(() => _statementsRepository.GetStatementTypeById(id)).Returns(statementType);

            //Act
            var result = await _statementsController.CreateStatement(id);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task StatementsController_HttpPost_CreateStatement_ReturnsRedirectToActionResult()
        {
            //Arrange
            var model = A.Fake<CreateStatementViewModel>();
            model.Image = A.Fake<IFormFile>();

            var claims = new[] { new Claim(ClaimTypes.Role, UserRoles.User), new Claim(ClaimTypes.Name, "fakeName") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _statementsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var user = A.Fake<AppUser>();

            A.CallTo(() => _statementsRepository.GetUserByUserName(A<string>._)).Returns(user);
            A.CallTo(() => _photoService.AddPhotoAsync(model.Image))
                .Returns(Task.FromResult(new ImageUploadResult { Url = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ") }));
            A.CallTo(() => _statementsRepository.AddStatement(A<UsersStatements>._)).Returns(true);

            //Act
            var result = await _statementsController.CreateStatement(model);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("StatementsList");
        }

        [Fact]
        public async Task StatementsController_DeleteUserStatement_ReturnsRedirectToActionResult()
        {
            //Arrange
            int id = 000;
            var userStatement = A.Fake<UsersStatements>();

            A.CallTo(() => _statementsRepository.GetUserStatementByStatementId(id)).Returns(userStatement);
            A.CallTo(() => _statementsRepository.RemoveStatementSignatureById(id)).Returns(true);
            A.CallTo(() => _statementsRepository.DeleteStatement(userStatement)).Returns(true);

            //Act
            var result = await _statementsController.DeleteUserStatement(id);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("MyStatements");
        }

        [Fact]
        public async Task StatementsController_UsersStatements_ReturnsViewResult()
        {
            //Arrange 
            var model = A.Fake<StatementsListViewModel>();

            A.CallTo(() => _statementsRepository.GetSignedStatements(A<string>._)).Returns(model);

            //Act
            var result = await _statementsController.UsersStatements();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task StatementsController_ChangeStatementStatus_ReturnsRedirectToActionResult()
        {
            //Arrange
            int id = 000;
            var statement = A.Fake<Statements>();
            var status = StatementsStatus.Ухвалена;

            A.CallTo(() => _statementsRepository.GetStatementByStatementId(id)).Returns(statement);
            A.CallTo(() => _statementsRepository.Update(statement)).Returns(true);

            //Act
            var result = await _statementsController.ChangeStatementStatus(id, status);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().BeOfType<RedirectToActionResult>()
                 .Which.ActionName.Should().Be("StatementDetails");
        }
    }
}
