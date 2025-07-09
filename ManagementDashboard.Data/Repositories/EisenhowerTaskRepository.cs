using System.Collections.Generic;
using System.Threading.Tasks;
using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;
using Microsoft.Extensions.Configuration;

namespace ManagementDashboard.Data.Repositories
{
    public interface IEisenhowerTaskRepository : IRepository<EisenhowerTask>
    {
        Task<IEnumerable<EisenhowerTask>> GetTasksByQuadrantAsync(string? quadrant);
    }

    public class EisenhowerTaskRepository : RepositoryBase<EisenhowerTask, EisenhowerTaskConstraints>, IEisenhowerTaskRepository
    {
        public EisenhowerTaskRepository(IConfiguration config) : base(config) { }

        public async Task<IEnumerable<EisenhowerTask>> GetTasksByQuadrantAsync(string? quadrant)
        {
            if (string.IsNullOrEmpty(quadrant))
            {
                // Use a custom constraint to fetch tasks where Quadrant is null or empty
                var constraints = new System.Collections.Hashtable { { "Uncategorized", true } };
                return await GetAsync(constraints);
            }
            return await GetAsync(new System.Collections.Hashtable { { "Quadrant", quadrant } });
        }
    }
}
