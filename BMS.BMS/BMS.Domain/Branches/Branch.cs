namespace BMS.Domain.Branches;

public class Branch
{
    public Guid Id { get; private set; }
    public string Name { get; init; } = string.Empty;
    public string ConnectionString { get;  init; } = string.Empty;
}