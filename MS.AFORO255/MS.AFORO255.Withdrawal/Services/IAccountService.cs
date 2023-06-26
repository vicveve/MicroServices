using MS.AFORO255.Withdrawal.Models;

namespace MS.AFORO255.Withdrawal.Services
{
    public interface IAccountService
    {
        Task<bool> DepositAccount(AccountRequest request);
        bool DepositReverse(TransactionModel request);
        bool Execute(TransactionModel request);
    }
}
