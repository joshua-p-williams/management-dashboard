namespace ManagementDashboard.Data.Models
{
    public interface ISoftDeleteEntity
    {
        DateTime? DeletedAt { get; set; }
        Boolean IsDeleted { get; set; }
    }
}
