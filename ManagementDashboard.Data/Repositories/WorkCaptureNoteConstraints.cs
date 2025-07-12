using Formula.SimpleRepo;
using ManagementDashboard.Core.Services;
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
            builder.Where("DATE(CreatedAt) = date(@Date)");
            parameters.Add("Date", DataTransformationService.ToSqliteDateString(Value));

            return parameters;
        }
    }
}
