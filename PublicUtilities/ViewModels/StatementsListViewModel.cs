using PublicUtilities.Data.Enum;

namespace PublicUtilities.ViewModels
{
    public class StatementsListViewModel
    {
        public string DepartamentName { get; set; }
        public ICollection<StatementsList> StatementsInfos { get; set; }

    }

    public class StatementsList
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public StatementsStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}
