using AMS.Common.Constants;

namespace AMS.Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }
    public Guid UserId { get; init; }
    public string Name { get; set; } = string.Empty;
    public int NumberOfAccounts { get; private set; }
    
    public void IncrementAccountCount()
    {
        if (NumberOfAccounts == CustomerConstants.MaxNumberOfAccounts)
        {
            throw new NumberOfAccountsExceeded(CustomerConstants.MaxNumberOfAccounts);
        }
        else
        {
            NumberOfAccounts++;
        }
    } 
    
    public void DecrementAccounts()
    {
        if (NumberOfAccounts > 0)
        {
            NumberOfAccounts--;
        }
        else
        {
            throw new InvalidOperationException("Number of accounts cannot be negative.");
        }
    }

}