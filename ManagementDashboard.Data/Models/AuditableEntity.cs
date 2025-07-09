using System;
using Dapper;
using Formula.SimpleRepo;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementDashboard.Data.Models
{
    public class AuditableEntity : IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? BlockerReason { get; set; }
        public DateTime? BlockedAt { get; set; }
        public DateTime? UnblockedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Dapper.NotMapped]
        public bool IsCompleted
        {
            get => CompletedAt != null;
            set
            {
                if (value)
                {
                    CompletedAt = DateTime.UtcNow;
                }
                else
                {
                    CompletedAt = null;
                }
            }
        }

        [Dapper.NotMapped]
        public bool IsBlocked
        {
            get => BlockedAt != null && UnblockedAt == null;
            set
            {
                if (value)
                {
                    BlockedAt = DateTime.UtcNow;
                    UnblockedAt = null;
                }
                else
                {
                    UnblockedAt = DateTime.UtcNow;
                }
            }
        }
    }
}