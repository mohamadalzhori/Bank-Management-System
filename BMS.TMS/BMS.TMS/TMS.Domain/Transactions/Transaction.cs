using System.Security.Principal;
using TMS.Domain.Accounts;

namespace TMS.Domain.Transactions;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; init; }
    public Account? Account { get; private set; }
    public long Amount { get; init; }
    public DateOnly Date { get; init;} 
}