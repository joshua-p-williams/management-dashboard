namespace ManagementDashboard.Data.Models
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? CompletedAt { get; set; }
        string? BlockerReason { get; set; }
        DateTime? BlockedAt { get; set; }
        DateTime? UnblockedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        bool IsCompleted { get; set; }
        bool IsBlocked { get; set; }
    }
}
