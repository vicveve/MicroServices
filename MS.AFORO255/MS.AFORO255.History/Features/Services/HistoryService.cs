using Aforo255.Cross.Metric.Registry;
using MongoDB.Driver;
using MS.AFORO255.History.Features.DTOs;
using MS.AFORO255.History.Features.Models;
using MS.AFORO255.History.Persistences;
using System.Text.Json;
using System.Transactions;

namespace MS.AFORO255.History.Features.Services;

public class HistoryService : IHistoryService
{
    private readonly IMongoBookDBContext _context;
    protected IMongoCollection<HistoryModel> _dbCollection;
    private readonly IMetricsRegistry _metricsRegistry;
    private readonly ILogger<HistoryService> _logger;
    public HistoryService(IMongoBookDBContext context, IMetricsRegistry metricsRegistry, ILogger<HistoryService> logger)
    {
        _context = context;
        _metricsRegistry = metricsRegistry;
        _dbCollection = _context.GetCollection<HistoryModel>(typeof(HistoryModel).Name);
        _logger = logger;
    }

    public async Task<bool> Add(HistoryModel historyModel)
    {
        await _dbCollection.InsertOneAsync(historyModel);
        return true;
    }

    public async Task<IEnumerable<HistoryResponse>> GetById(int accountId)
    {
        _logger.LogInformation("GET in HistoryService with {0}", JsonSerializer.Serialize(accountId));
        var result = await _dbCollection.Find(x=> x.AccountId == accountId).ToListAsync();
        var response = new List<HistoryResponse>();
        foreach (var item in result)
        {
            response.Add(new HistoryResponse()
            {
                AccountId = item.AccountId,
                Amount = item.Amount,
                CreationDate = item.CreationDate,
                IdTransaction = item.IdTransaction,
                Type = item.Type
            });
        }

        _metricsRegistry.IncrementFindQuery();
        return response;
    }
}
