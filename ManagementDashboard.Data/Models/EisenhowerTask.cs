using System;
using Dapper;
using Formula.SimpleRepo;

namespace ManagementDashboard.Data.Models
{
    [ConnectionDetails("DefaultConnection", typeof(Microsoft.Data.Sqlite.SqliteConnection), Dapper.SimpleCRUD.Dialect.SQLite)]
    [Table("Tasks")]
    public class EisenhowerTask : IAuditableEntity
    {
        [Dapper.Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Quadrant { get; set; } // Do, Schedule, Delegate, Delete, or null
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsBlocked { get; set; }
        public string? BlockerReason { get; set; }
        public DateTime? BlockedAt { get; set; }
        public DateTime? UnblockedAt { get; set; }
        public string? DelegatedTo { get; set; }
        public PriorityLevel Priority { get; set; } // 0=Low, 1=Medium, 2=High
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? CompletedAt { get; set; }
        bool IsBlocked { get; set; }
        string? BlockerReason { get; set; }
        DateTime? BlockedAt { get; set; }
        DateTime? UnblockedAt { get; set; }
    }

    public enum PriorityLevel { Low = 0, Medium = 1, High = 2 }
}
