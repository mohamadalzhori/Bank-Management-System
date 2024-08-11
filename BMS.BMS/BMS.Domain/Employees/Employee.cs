namespace BMS.Domain.Employees;

public class Employee
{
    public Guid Id { get; private set; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
}