using Formula.SimpleRepo;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementDashboard.Data.Models
{
    [ConnectionDetails("DefaultConnection", typeof(Microsoft.Data.Sqlite.SqliteConnection), Dapper.SimpleCRUD.Dialect.SQLite)]
    [Table("WorkCaptureNotes")]
    public class WorkCaptureNote
    {
        [Dapper.Key]
        public int Id { get; set; }
        public string? Notes { get; set; }
        public int? TaskId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
