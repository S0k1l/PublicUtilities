﻿using PublicUtilities.Models;
using PublicUtilities.ViewModels;

namespace PublicUtilities.Interface
{
    public interface IStatementsRepository
    {
        Task<ICollection<StatementsTypeListViewModel>> getAllStatements();
        Task<StatementsTypes> GetStatementTypeById(int id);
        Task<StatementDetailsViewModel> GetStatementDetailsById(int id);
        Task<AppUser> GetUserByUserName(string userName);
        Task<ICollection<MyStatements>> GetUserStatementByUserName(string userName);
        Task<ICollection<MyStatements>> GetOtherUserStatementByUserName(string userName);
        Task<UsersStatements> GetUserStatementByStatementId(int id);
        Task<Statements> GetStatementByStatementId(int id);
        bool AddSignatureTostatement(Statements statements);
        Task<bool> RemoveStatementSignatureById(int statementId);
        Task<StatementsListViewModel> GetSignedStatements(string departament);
        bool AddStatement(UsersStatements statements);
        bool DeleteStatement(UsersStatements statements);
        bool Update(Statements statements);
        bool Save();
    }
}
