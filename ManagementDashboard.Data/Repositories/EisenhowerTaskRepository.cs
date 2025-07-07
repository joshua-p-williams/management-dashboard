using System.Collections.Generic;
using System.Threading.Tasks;
using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;
using Microsoft.Extensions.Configuration;

namespace ManagementDashboard.Data.Repositories
{
    public interface IEisenhowerTaskRepository
    {
        Task AddTaskAsync(EisenhowerTask task);
        Task<IEnumerable<EisenhowerTask>> GetTasksByQuadrantAsync(string? quadrant);
        Task UpdateTaskAsync(EisenhowerTask task);
        // Optionally: Task<EisenhowerTask?> GetByIdAsync(int id);
    }

    public class EisenhowerTaskRepository : RepositoryBase<EisenhowerTask, EisenhowerTask>, IEisenhowerTaskRepository
    {
        public EisenhowerTaskRepository(IConfiguration config) : base(config) { }

        public async Task AddTaskAsync(EisenhowerTask task)
        {
            await InsertAsync(task);
        }

        public async Task<IEnumerable<EisenhowerTask>> GetTasksByQuadrantAsync(string? quadrant)
        {
            if (string.IsNullOrEmpty(quadrant))
                return await GetAsync(new System.Collections.Hashtable { { "Quadrant", null } });
            return await GetAsync(new System.Collections.Hashtable { { "Quadrant", quadrant } });
        }

        public async Task UpdateTaskAsync(EisenhowerTask task)
        {
            await UpdateAsync(task);
        }
    }
}
