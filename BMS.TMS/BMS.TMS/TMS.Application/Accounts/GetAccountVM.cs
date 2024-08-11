namespace TMS.Application.Accounts;

public class GetAccountVM
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public decimal Balance { get; set; }
}