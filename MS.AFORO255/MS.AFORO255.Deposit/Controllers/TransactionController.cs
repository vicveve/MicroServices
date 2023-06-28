using Aforo255.Cross.Event.Src.Bus;
using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Deposit.DTOs;
using MS.AFORO255.Deposit.Messages.Commands;
using MS.AFORO255.Deposit.Models;
using MS.AFORO255.Deposit.Services;
using Aforo255.Cross.Metric.Registry;
using System.Text.Json;

namespace MS.AFORO255.Deposit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IEventBus _eventBus;
        private readonly IAccountService _accountService;
        private readonly IMetricsRegistry _metricsRegistry;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, IEventBus eventBus, IAccountService accountService, IMetricsRegistry metricsRegistry, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _eventBus = eventBus;
            _accountService = accountService;
            _metricsRegistry = metricsRegistry;
            _logger = logger;
        }


        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] TransactionRequest request)
        {
            _logger.LogInformation("POST in TransactionController with {0}", JsonSerializer.Serialize(request));
            TransactionModel transaction = new TransactionModel(request.Amount, request.AccountId);
            transaction = _transactionService.Deposit(transaction);
            if (_accountService.Execute(transaction))
            {
                _eventBus.SendCommand(new TransactionCreateCommand(transaction.Id, transaction.Amount, transaction.Type,
                    transaction.CreationDate, transaction.AccountId));
                _eventBus.SendCommand(new NotificationCreateCommand(transaction.Id, transaction.Amount, transaction.Type,
                       $"Se proceso el {transaction.Type} con el monto de {transaction.Amount} de su cuenta {transaction.AccountId}",
                       "servicedesk@aforo255.com", transaction.AccountId));

            }

            _metricsRegistry.IncrementFindQuery();


            return Ok(transaction);
        }
    }
}
