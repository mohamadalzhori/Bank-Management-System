namespace AMS.Domain.Accounts;

public class Account
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; init; }
}