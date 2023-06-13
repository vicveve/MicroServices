using Aforo255.Cross.Event.Src;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Withdrawal.Messages.CommandHandlers;
using MS.AFORO255.Withdrawal.Messages.Commands;
using MS.AFORO255.Withdrawal.Models;
using MS.AFORO255.Withdrawal.Persistences;
using MS.AFORO255.Withdrawal.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ContextDatabase>(
    options =>
    {
        options.UseNpgsql(builder.Configuration["postgres:cn"]);
    });
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
builder.Services.AddRabbitMQ();
builder.Services.AddTransient<IRequestHandler<TransactionCreateCommand, bool>, TransactionCommandHandler>();

var app = builder.Build();
app.MapPost("/api/transaction/withdrawal", (TransactionRequest request, ITransactionService transactionService) =>
{
    TransactionModel transaction = new TransactionModel(request.Amount, request.AccountId);
    transaction = transactionService.Withdrawal(transaction);
    return transaction;
})
.Produces<List<TransactionModel>>()
.WithName("Withdrawal");
app.Run();

internal record TransactionRequest(int AccountId, decimal Amount);
