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

        // Returns true if the task is past its due date (if set)
        [Dapper.NotMapped]
        public bool IsPastDue
        {
            get => DueDate.HasValue && DueDate.Value.Date < DateTime.Now.Date;
            set { }
        }

        // Returns a summary string for the due date (e.g. "Due in 3 days (2024-06-10)")
        [Dapper.NotMapped]
        public string DueDateSummary
        {
            get
            {
                if (!DueDate.HasValue) return "No due date";
                var days = (DueDate.Value.Date - DateTime.Now.Date).Days;
                if (days > 0) return $"Due in {days} day{(days == 1 ? "" : "s")} ({DueDate.Value:yyyy-MM-dd})";
                if (days == 0) return $"Due today ({DueDate.Value:yyyy-MM-dd})";
                return $"Past due ({DueDate.Value:yyyy-MM-dd})";
            }
            set { }
        }

        // Returns true if the due date is within the reminder threshold (and not past due)
        public bool IsDueDateReminder(int dueDateReminderThresholdDays)
        {
            if (!DueDate.HasValue || IsPastDue) return false;
            var daysUntilDue = (DueDate.Value.Date - DateTime.Now.Date).TotalDays;
            return daysUntilDue >= 0 && daysUntilDue <= dueDateReminderThresholdDays;
        }
    }

    public enum PriorityLevel { Low = 0, Medium = 1, High = 2 }
}
