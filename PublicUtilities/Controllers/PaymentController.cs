using Microsoft.AspNetCore.Mvc;
using PublicUtilities.Interface;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Water()
        {
            var model = await _paymentRepository.GetUserPaymentByUserName(User.Identity.Name, "Вода");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Gas()
        {
            var model = await _paymentRepository.GetUserPaymentByUserName(User.Identity.Name, "Газ");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Electricity()
        {
            var model = await _paymentRepository.GetUserPaymentByUserName(User.Identity.Name, "Електроенергія");
            return View(model);
        }

        [HttpGet]
        public IActionResult PaymentType(int id, string placeOfResidence, string price, string date)
        {
            var model = new PaymentViewModel
            {
                Id = id,
                PlaceOfResidence = placeOfResidence,
                Price = price,
                Date = date,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentType(int id)
        {
            var indicator = await _paymentRepository.GetUserPaymentById(id);
            
            if(indicator == null) { return RedirectToAction("Water"); }

            if (_paymentRepository.PeymetntOperation(indicator)) { return RedirectToAction("Water"); }

            return View();
        }
    }
}
