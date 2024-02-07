using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
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
                    .Where(i => i.Paid == false && i.PlacesOfResidenceId == item.PlacesOfResidenceId && i.Utilities.Name == service)
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
            indicators.Paid = true;
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
