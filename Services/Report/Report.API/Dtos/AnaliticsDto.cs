namespace Report.API.Dtos;

public record AnaliticsDto
{
    public int OverdueTasks { get; set; }
    public int BudgetUsage { get; set; }
    public int ProjectCompletitionRate { get; set; }
    public int ProjectsPendingRate { get; set; }
    public int Professionals { get; set; } //Who belongs to that tenant and assigned to projects/tasks
    public int ActiveProjects { get; set; }
    public int PendingProjects { get; set; }
    public IEnumerable<ProjectDto> Projects { get; set; } = default!;
    public IEnumerable<Professional> ProfessionalsOfTheDay { get; set; } = default!;
}

public record ProjectDto
{
    public string Name { get; set; } = default!;
    public DateTime EndDate { get; set; }
}

public record Professional
{
    public string Name { get; set; } = default!;
    public string Image { get; set; } = default!;
}

public record ActivePendingTasks
{
    public int ActiveProjects { get; set; }
    public int PendingProjects { get; set; }

    public int TotalProjects { get; set; }
}

public record TasksCount
{
    public int ComplitedTasks { get; set; }
    public int TotalTasks { get; set; }
}
