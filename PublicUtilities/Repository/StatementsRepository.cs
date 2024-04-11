using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data;
using PublicUtilities.Interface;
using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Repository
{
    public class StatementsRepository : IStatementsRepository
    {
        private readonly AppDbContext _context;

        public StatementsRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool AddSignatureTostatement(Statements statements)
        {
            statements.SignarureCount += 1;
            _context.Update(statements);
            return Save();
        }

        public bool AddStatement(UsersStatements statements)
        {
            _context.Add(statements);
            return Save();
        }

        public bool DeleteStatement(UsersStatements statements)
        {
            _context.Remove(statements);
            return Save();
        }

        public async Task<ICollection<StatementsTypeListViewModel>> getAllStatements()
        {
            var depertaments = await _context.Departments.ToListAsync();
            var statements = new List<StatementsTypeListViewModel>();

            foreach (var item in depertaments)
            {
                var statement = await _context.StatementsTypes
                    .Where(s => s.Department == item)
                    .Select(s => new StatementsTyprInfo
                    {
                        StatementId = s.Id,
                        StatementType = s.Type,
                    }).ToListAsync();
                statements.Add(
                    new StatementsTypeListViewModel
                    {
                        DepartamentName = item.Name,
                        StatementsInfos = statement,
                    });
            }

            return statements;
        }

        public async Task<ICollection<MyStatements>> GetOtherUserStatementByUserName(string userName)
        {
           var streets = await _context.UsersPlacesOfResidence
                .Where(upor => upor.AppUser.UserName == userName)
                .Select(upor => upor.PlacesOfResidence.Streets)
                .ToListAsync();

            var user = await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            var userStatements = await _context.UsersStatements
                .Where(us => us.AppUserId == user.Id)
                .Select(us => new MyStatements
                {
                    Id = us.Statements.Id,
                    StatementsType = us.Statements.StatementsType.Type,
                    Status = us.Statements.Status,
                    Date = us.Statements.Date,
                })
                .ToListAsync();

            var myStatements = new List<MyStatements>();
            foreach (var item in streets)
            {
                var statements = await _context.UsersStatements
                    .Where(us => us.Statements.Street == item.Name
                    && us.Statements.SignarureCount < us.Statements.StatementsType.SignatureCount
                    && us.Statements.StatementsType.isStreetNeeded == true
                    && us.AppUserId != user.Id)
                    .Select(us => new MyStatements
                    {
                        Id = us.Statements.Id,
                        StatementsType = us.Statements.StatementsType.Type,
                        Status = us.Statements.Status,
                        Date = us.Statements.Date,
                    })
                .OrderBy(ms => ms.Date)
                .ToListAsync();
                myStatements.AddRange(statements);
            }

            foreach (var item in userStatements)
            {
                for (int i = 0; i < myStatements.Count; i++)
                {
                    if (myStatements[i].Id == item.Id)
                    {
                        myStatements.RemoveAt(i);
                    }
                }
            }

            return myStatements;
        }

        public async Task<StatementsListViewModel> GetSignedStatements(string departament)
        {
                var statement = await _context.UsersStatements
                    .Where(s => s.Statements.StatementsType.Department.Name == departament 
                    && s.Statements.SignarureCount >= s.Statements.StatementsType.SignatureCount)
                    .Select(s => new StatementsList
                    {
                        Id = s.Statements.Id,
                        Status = s.Statements.Status,
                        Date = s.Statements.Date,
                        Surname = s.AppUser.Surname,
                        Name = s.AppUser.Name,
                        Patronymic = s.AppUser.Patronymic,
                    }).ToListAsync();
            var statements = new StatementsListViewModel
                    {
                        DepartamentName = departament,
                        StatementsInfos = statement,
                    };
            return statements;
        }

        public async Task<Statements> GetStatementByStatementId(int id)
        {
            return await _context.Statements.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<StatementDetailsViewModel> GetStatementDetailsById(int id)
        {
            return await _context.Statements
                .Where(s => s.Id == id)
                .Select(s => new StatementDetailsViewModel
                {
                    Id = s.Id,
                    StatementsType = s.StatementsType.Type,
                    Street = s.Street,
                    Text = s.Text,
                    StatementUrl = s.StatementUrl,
                    Status = s.Status,
                    Date = s.Date.ToShortDateString(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StatementsTypes> GetStatementTypeById(int id)
        {
            return await _context.StatementsTypes.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<UsersStatements> GetUserStatementByStatementId(int id)
        {
            return await _context.UsersStatements.FirstOrDefaultAsync(us => us.StatementsId == id);
        }

        public async Task<ICollection<MyStatements>> GetUserStatementByUserName(string userName)
        {
            var statements = await _context.UsersStatements
                .Where(us => us.AppUser.UserName == userName)
                .Select(us => new MyStatements 
                { 
                    Id = us.Statements.Id,
                    StatementsType = us.Statements.StatementsType.Type,
                    Status = us.Statements.Status,
                    Date = us.Statements.Date,
                })
                .OrderBy(msvm => msvm.Date)
                .ToListAsync();
            return statements;
        }

        public async Task<bool> RemoveStatementSignatureById(int statementId)
        {
            var statement = await _context.Statements
                .Where(s => s.Id == statementId)
                .FirstOrDefaultAsync();

            statement.SignarureCount -= 1;

            _context.Update(statement);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Statements statements)
        {
            _context.Update(statements);
            return Save();
        }
    }
}