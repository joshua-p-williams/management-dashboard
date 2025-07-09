using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;

namespace ManagementDashboard.Data.Repositories
{
    // Constraint model for EisenhowerTask with custom constraint for 'Uncategorized'
    public class EisenhowerTaskConstraints : EisenhowerTask
    {
        public UncategorizedConstraint? Uncategorized { get; set; }
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
}
