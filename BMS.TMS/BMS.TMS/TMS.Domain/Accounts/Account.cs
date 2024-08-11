using TMS.Domain.Transactions;

namespace TMS.Domain.Accounts;

public class Account
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; init; }
    public long AccountBalance { get; set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
    
    private void Withdraw(long amount)
    {
        if (AccountBalance < Math.Abs(amount))
        {
            throw new InsufficientBalance(Math.Abs(amount));
        }
        
        AccountBalance -= Math.Abs(amount);
    }
    
    private void Deposit(long amount)
    {
        AccountBalance += amount;
    }
    
    public void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount >= 0)
        {
           this.Deposit(transaction.Amount); 
        }else if (transaction.Amount < 0)
        {
            this.Withdraw(transaction.Amount);
        }
        
        _transactions.Add(transaction);
    }
}