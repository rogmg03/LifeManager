namespace LifeManager.Domain.Entities;

public class WorkInitiativeDetail
{
    public Guid ProjectId { get; set; }
    public string? ClientName { get; set; }
    public decimal? ContractValue { get; set; }
    public string? BillingType { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? LoggedHours { get; set; }
    public bool IsInternal { get; set; } = false;

    // Navigation
    public Project Project { get; set; } = null!;
}
