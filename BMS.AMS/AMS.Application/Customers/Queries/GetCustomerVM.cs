namespace AMS.Application.Customers.Queries;

public class GetCustomerVM
{
    public Guid Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public int NumberOfAccounts { get; private set; }
}