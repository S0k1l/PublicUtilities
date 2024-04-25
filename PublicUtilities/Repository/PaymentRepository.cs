using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PublicUtilities.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PaymentListViewModel>> GetPaymentList()
        {
            int currentYear = DateTime.Today.Year;
            int currentMonth = DateTime.Today.Month;
            int previousMonth = currentMonth - 1;
            if (previousMonth == 0)
            {
                previousMonth = 12;
            }

            var model = await _context.Indicators.Where(i =>i.Date.Year == currentYear && i.Date.Month == currentMonth && i.isPaid == false)
            .Select(i => new PaymentListViewModel
            {
                PlaceOfResidenceId = i.PlacesOfResidenceId,
                PlaceOfResidence = $"Вул. {i.PlacesOfResidence.Streets.Name}, буд. {i.PlacesOfResidence.House}, кв. {i.PlacesOfResidence.Apartment}",
                UtiltiesName = i.Utilities.Name,
                isPaid = i.isPaid,
                Date = i.Date,
            })
            .ToListAsync();

            return model;
        }

        public async Task<Indicators> GetUserPaymentById(int id)
        {
            return await _context.Indicators.Where(i =>  i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<PaymentViewModel>> GetUserPaymentByUserName(string userName, string service)
        {
            var userPlaceOfResidence = await _context.UsersPlacesOfResidence.Where(u  => u.AppUser.UserName == userName).ToListAsync();

            var payments = new List<PaymentViewModel>();

            foreach (var item in userPlaceOfResidence)
            {
                var payment = await _context.Indicators
                    .Where(i => i.isPaid == false && i.PlacesOfResidenceId == item.PlacesOfResidenceId && i.Utilities.Name == service)
                    .Select(i => new PaymentViewModel
                    {
                        Id = i.Id,
                        Price = i.Price.ToString(),
                        PlaceOfResidence = $"Вул. {i.PlacesOfResidence.Streets.Name}, " +
                            $"буд. {i.PlacesOfResidence.House}, " +
                            $"кв. {i.PlacesOfResidence.Apartment}",
                        Date = i.Date.ToShortDateString(),
                    })
                    .ToListAsync();
                payments.AddRange(payment);
            }

            return payments;
        }

        public bool PeymetntOperation(Indicators indicators)
        {
            indicators.isPaid = true;
            indicators.PaymentDate = DateTime.Now;
            _context.Update(indicators);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
