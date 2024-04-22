namespace PublicUtilities.ViewModels
{
    public class EditNewsViewModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public IFormFile? Image { get; set; }
    }
}
