using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.Services;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class StatementsController : Controller
    {
        private readonly IStatementsRepository _statementsRepository;
        private readonly IPhotoService _photoService;

        public StatementsController(IStatementsRepository statementsRepository, IPhotoService photoService)
        {
            _statementsRepository = statementsRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> MyStatements(string alert = null)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }
            
            var myStatements = await _statementsRepository.GetUserStatementByUserName(User.Identity.Name);
            var otherStatements = await _statementsRepository.GetOtherUserStatementByUserName(User.Identity.Name);
            var model = new MyStatementsViewModel
            {
                MyStatements = myStatements,
                OthersStatements = otherStatements,
            };
            if (!alert.IsNullOrEmpty())
            {
                ViewBag.AlertMessage = alert;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StatementDetails(int id)
        {
            var statement = await _statementsRepository.GetStatementDetailsById(id);
            return View(statement);
        }

        [HttpGet]
        public async Task<IActionResult> SignStatement(int id)
        {
            var statement = await _statementsRepository.GetStatementDetailsById(id);
            return View(statement);
        }

        [HttpPost("SignStatement")]
        public async Task<IActionResult> SignStatementPost(int id)
        {
            var statement = await _statementsRepository.GetStatementByStatementId(id);
            
            if (!_statementsRepository.AddSignatureTostatement(statement))
            {
                return RedirectToAction("MyStatements");
            }

            var user = await _statementsRepository.GetUserByUserName(User.Identity.Name);
            var userStatement = new UsersStatements
            {
                AppUserId = user.Id,
                AppUser = user,
                Statements = statement,
                StatementsId = statement.Id,
            };

            if (_statementsRepository.AddStatement(userStatement))
            {
                return RedirectToAction("MyStatements", new { alert = "Заяву успішно підписано!" });
            }

            return RedirectToAction("MyStatements");
        }

        [HttpGet]
        public async Task<IActionResult> StatementsList(string alert = null)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }

            var model = await _statementsRepository.getAllStatements();

            if (!alert.IsNullOrEmpty())
            {
                ViewBag.AlertMessage = alert;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateStatement(int id)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Account"); }

            var statementType = await _statementsRepository.GetStatementTypeById(id);
            var model = new CreateStatementViewModel
            {
                StatementId = statementType.Id,
                StatementType = statementType.Type,
                isPhotoNeeded = statementType.isPhotoNeeded,
                isStreetNeeded = statementType.isStreetNeeded,
                SignatureCount = statementType.SignatureCount,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatement(CreateStatementViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var user = await _statementsRepository.GetUserByUserName(User.Identity.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувача не існує");
                return View(model);
            }

            string statementUrl = null;

            if (model.Image != null)
            {
                var result = await _photoService.AddPhotoAsync(model.Image);
                statementUrl = result.Url.ToString();
            }

            var statement = new UsersStatements
            {
                AppUser = user,
                Statements = new Statements
                {
                    StatementsTypeId = model.StatementId,
                    Street = model.Street,
                    Text = model.Text,
                    StatementUrl = statementUrl,
                    Status = Data.Enum.StatementsStatus.Зареєстровано,
                    Date = DateTime.Now,
                    SignarureCount = 1,
                }
            };

            if (_statementsRepository.AddStatement(statement))
            {
                return RedirectToAction("StatementsList", new { alert = "Заяву успішно подано!" });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserStatement(int id)
        {
            var userStatement = await _statementsRepository.GetUserStatementByStatementId(id);
            if(!await _statementsRepository.RemoveStatementSignatureById(id)) 
            { 
                return RedirectToAction("MyStatements"); 
            }

            if(_statementsRepository.DeleteStatement(userStatement))
            {
                return RedirectToAction("MyStatements", new { alert = "Заяву успішно видалено!" });
            }

            return RedirectToAction("MyStatements");
        }
    }
}
