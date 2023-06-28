using Aforo255.Cross.Event.Src.Bus;
using Aforo255.Cross.Metric.Registry;
using MediatR;
using MS.AFORO255.Withdrawal.Messages.Commands;
using MS.AFORO255.Withdrawal.Models;
using MS.AFORO255.Withdrawal.Persistences;
using Nacos;
using System.Text.Json;

namespace MS.AFORO255.Withdrawal.Services;

public class TransactionService : ITransactionService
{
    private readonly ContextDatabase _contextDatabase;
    private readonly IEventBus _eventBus;
    private readonly IMetricsRegistry _metricsRegistry;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(ContextDatabase contextDatabase, IEventBus eventBus, IMetricsRegistry metricsRegistry, ILogger<TransactionService> logger)
    {
        _contextDatabase = contextDatabase;
        _eventBus = eventBus;
        _metricsRegistry = metricsRegistry;
        _logger = logger;
    }

    public TransactionModel Withdrawal(TransactionModel transaction)
    {
        _logger.LogInformation("Post in TransactionService with {0}", JsonSerializer.Serialize(transaction));
        _contextDatabase.Transaction.Add(transaction);
        _contextDatabase.SaveChanges();
        _metricsRegistry.IncrementFindQuery();
        return transaction;
    }

    public TransactionModel WithdrawalReverse(TransactionModel transaction)
    {
        _contextDatabase.Transaction.Add(transaction);
        _contextDatabase.SaveChanges();
        return transaction;
    }
}

