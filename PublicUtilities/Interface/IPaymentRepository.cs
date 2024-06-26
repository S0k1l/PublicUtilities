﻿using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IPaymentRepository
    {
        Task<ICollection<PaymentViewModel>> GetUserPaymentByUserName(string userName , string service);
        Task<Indicators> GetUserPaymentById(int id);
        Task<ICollection<PaymentListViewModel>> GetPaymentList();
        bool PeymetntOperation(Indicators indicators);
        bool Save();
    }
}
