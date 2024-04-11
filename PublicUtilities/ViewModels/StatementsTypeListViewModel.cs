namespace PublicUtilities.ViewModels
{
    public class StatementsTypeListViewModel
    {
        public string DepartamentName { get; set; }
        public ICollection<StatementsTyprInfo> StatementsInfos { get; set; }
    }

    public class StatementsTyprInfo
    {
        public int StatementId { get; set; }
        public string StatementType { get; set; }
    }
}
