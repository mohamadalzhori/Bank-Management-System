﻿namespace TMS.Domain.Branches;

public class Branch
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ConnectionString { get;  init; } = string.Empty;
}