using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;

namespace ManagementDashboard.Data.Repositories
{
    // Constraint model for EisenhowerTask with custom constraint for 'Uncategorized'
    public class EisenhowerTaskConstraints : EisenhowerTask
    {
        public UncategorizedConstraint? Uncategorized { get; set; }
        public CurrentTasksConstraint? CurrentTasks { get; set; }
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
}
