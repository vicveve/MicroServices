namespace MS.AFORO255.Withdrawal.Models
{
    public class AccountRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }

        public AccountRequest(int accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }

    }
}
