﻿namespace Company.Project.Core.Cqrs;

public class IntervalQuery
{
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
}
