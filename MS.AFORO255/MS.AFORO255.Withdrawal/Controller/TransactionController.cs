using Aforo255.Cross.Event.Src.Bus;
using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Withdrawal.Messages.Commands;
using MS.AFORO255.Withdrawal.Models;
using MS.AFORO255.Withdrawal.Services;

namespace MS.AFORO255.Withdrawal.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IEventBus _eventBus;

        public TransactionController(ITransactionService transactionService, IEventBus eventBus)
        {
            _transactionService = transactionService;
            _eventBus = eventBus;
        }

        [HttpPost("withdrawal")]
        public IActionResult Withdrawal([FromBody] TransactionRequest request)
        {
            TransactionModel transaction = new TransactionModel(request.Amount, request.AccountId);
            transaction = _transactionService.withdrawal(transaction);

            _eventBus.SendCommand(new TransactionCreateCommand(transaction.Id, transaction.Amount, transaction.Type,
                   transaction.CreationDate, transaction.AccountId));

            return Ok(transaction);
        }
    }
}
