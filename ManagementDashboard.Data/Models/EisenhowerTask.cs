using System;
using Dapper;
using Formula.SimpleRepo;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementDashboard.Data.Models
{
    [ConnectionDetails("DefaultConnection", typeof(Microsoft.Data.Sqlite.SqliteConnection), Dapper.SimpleCRUD.Dialect.SQLite)]
    [Dapper.Table("Tasks")]
    public class EisenhowerTask : AuditableEntity, IAuditableEntity, ISoftDeleteEntity
    {
        [Dapper.Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Quadrant { get; set; } // Do, Schedule, Delegate, Delete, or null
        public string? DelegatedTo { get; set; }
        public PriorityLevel Priority { get; set; } // 0=Low, 1=Medium, 2=High
        public DateTime? DeletedAt { get; set; }
        public DateTime? DueDate { get; set; }

        [Dapper.NotMapped]
        public Boolean IsDeleted
        {
            get => DeletedAt != null;
            set
            {
                if (value)
                {
                    DeletedAt = DateTime.Now;
                }
                else
                {
                    DeletedAt = null;
                }
            }
        }

        public Boolean IsPastDue(int overdueThresholdDays)
        {
            var pastDue = false;
            if (DueDate.HasValue)
            {
                pastDue = DueDate.Value.Date < DateTime.Now.Date;
            }
            return pastDue;
        }
    }

    public enum PriorityLevel { Low = 0, Medium = 1, High = 2 }
}
