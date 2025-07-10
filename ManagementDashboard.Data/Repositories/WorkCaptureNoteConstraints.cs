using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;
using System;
using System.Collections.Generic;

namespace ManagementDashboard.Data.Repositories
{
    // Constraint model for WorkCaptureNote (add custom constraints as needed)
    public class WorkCaptureNoteConstraints : WorkCaptureNote
    {
        public CreatedOnDateConstraint? CreatedOnDate { get; set; }
    }

    // Custom constraint: filter notes by date (date part only)
    public class CreatedOnDateConstraint : Constraint
    {
        public override Dictionary<string, object> Bind(Dapper.SqlBuilder builder)
        {
            var parameters = new Dictionary<string, object>();

            DateTime dateValue;
            // Let's do it safely with DateTime.TryParse
            if (!DateTime.TryParse(this.Value.ToString(), out dateValue))
            {
                throw new ArgumentException("Invalid date format. Please provide a valid date.");
            }

            // Format as "yyyy-MM-dd"
            string formattedDate = dateValue.ToString("yyyy-MM-dd");

            builder.Where("DATE(CreatedAt) = date(@Date)");
            parameters.Add("Date", formattedDate);

            return parameters;
        }
    }
}
