using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Deposit.DTOs;
using MS.AFORO255.Deposit.Models;
using MS.AFORO255.Deposit.Services;

namespace MS.AFORO255.Deposit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService) => _transactionService = transactionService;

        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] TransactionRequest request)
        {
            TransactionModel transaction = new TransactionModel(request.Amount, request.AccountId);
            transaction = _transactionService.Deposit(transaction);

            return Ok(transaction);
        }
    }
}
