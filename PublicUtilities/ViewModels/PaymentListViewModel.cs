namespace PublicUtilities.ViewModels
{
    public class PaymentListViewModel
    {
        public int PlaceOfResidenceId { get; set; }
        public string PlaceOfResidence { get; set; }
        public string UtiltiesName { get; set; }
        public bool isPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
