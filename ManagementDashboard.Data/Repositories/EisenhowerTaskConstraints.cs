using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;

namespace ManagementDashboard.Data.Repositories
{
    // Constraint model for EisenhowerTask with custom constraint for 'Uncategorized'
    public class EisenhowerTaskConstraints : EisenhowerTask
    {
        public UncategorizedConstraint? Uncategorized { get; set; }
        public CurrentTasksConstraint? CurrentTasks { get; set; }
        public WorkedOnDateConstraint? WorkedOnDate { get; set; }
        public BlockedConstraint? Blocked { get; set; }
    }

    // Custom constraint: returns tasks where Quadrant is null or empty
    public class UncategorizedConstraint : Constraint
    {
        public override Dictionary<string, object> Bind(Dapper.SqlBuilder builder)
        {
            var parameters = new Dictionary<string, object>();
            builder.Where("(Quadrant IS NULL OR Quadrant = '')");
            return parameters;
        }
    }

    // Custom constraint: returns tasks that are not deleted and not completed before today
    public class CurrentTasksConstraint : Constraint
    {
        public override Dictionary<string, object> Bind(Dapper.SqlBuilder builder)
        {
            var parameters = new Dictionary<string, object>();
            builder.Where("(DeletedAt IS NULL AND (CompletedAt IS NULL OR date(CompletedAt) >= date(@Today)))");
            parameters.Add("Today", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            return parameters;
        }
    }

    // Custom constraint: returns tasks worked on a specific date (UpdatedAt or CompletedAt)
    public class WorkedOnDateConstraint : Constraint
    {
        public override Dictionary<string, object> Bind(Dapper.SqlBuilder builder)
        {
            var parameters = new Dictionary<string, object>();
            var date = Value is DateTime dt ? dt.ToString("yyyy-MM-dd") : Value?.ToString();
            builder.Where("(date(UpdatedAt) = date(@WorkedOnDate) OR date(CompletedAt) = date(@WorkedOnDate) OR date(BlockedAt) = date(@WorkedOnDate) OR date(CreatedAt) = date(@WorkedOnDate) OR date(UnblockedAt) = date(@WorkedOnDate) OR date(DeletedAt) = date(@WorkedOnDate))");
            parameters.Add("WorkedOnDate", date);
            return parameters;
        }
    }

    public class BlockedConstraint : Constraint
    {
        public override Dictionary<string, object> Bind(Dapper.SqlBuilder builder)
        {
            var parameters = new Dictionary<string, object>();
            if (Value is bool isBlocked)
            {
                if (isBlocked)
                {
                    builder.Where("BlockedAt IS NOT NULL AND UnblockedAt IS NULL");
                }
                else
                {
                    builder.Where("BlockedAt IS NULL OR UnblockedAt IS NOT NULL");
                }
            }
            // If Value is null, do not add any filter
            return parameters;
        }
    }
}
