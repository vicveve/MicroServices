using Aforo255.Cross.Event.Src.Bus;
using MediatR;
using MS.AFORO255.Withdrawal.Messages.Commands;
using MS.AFORO255.Withdrawal.Models;
using MS.AFORO255.Withdrawal.Persistences;

namespace MS.AFORO255.Withdrawal.Services;

public class TransactionService : ITransactionService
{
    private readonly ContextDatabase _contextDatabase;
    private readonly IEventBus _eventBus;

    public TransactionService(ContextDatabase contextDatabase, IEventBus eventBus)
    {
        _contextDatabase = contextDatabase;
        _eventBus = eventBus;
    }

    public TransactionModel Withdrawal(TransactionModel transaction)
    {
        _contextDatabase.Transaction.Add(transaction);
        _contextDatabase.SaveChanges();
        return transaction;
    }

    public TransactionModel WithdrawalReverse(TransactionModel transaction)
    {
        _contextDatabase.Transaction.Add(transaction);
        _contextDatabase.SaveChanges();
        return transaction;
    }
}

