using System.Collections;
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
        Task<int> CompleteTaskAsync(EisenhowerTask task);
        Task<int> MoveTaskToQuadrantAsync(EisenhowerTask task, string quadrant);
        Task<IEnumerable<EisenhowerTask>> GetWorkedOnByDateAsync(DateTime date);
        Task<IEnumerable<EisenhowerTask>> GetBlockedAsync();
    }

    public class EisenhowerTaskRepository : RepositoryBase<EisenhowerTask, EisenhowerTaskConstraints>, IEisenhowerTaskRepository
    {
        public EisenhowerTaskRepository(IConfiguration config) : base(config) { }

        public override List<Formula.SimpleRepo.Constraint> ScopedConstraints(List<Formula.SimpleRepo.Constraint> currentConstraints)
        {
            var constraints = new Hashtable();
            constraints.Add("DeletedAt", null); // Only include tasks that are not deleted
            return this.GetConstraints(constraints);
        }

        public async Task<IEnumerable<EisenhowerTask>> GetTasksByQuadrantAsync(string? quadrant)
        {
            var constraints = new System.Collections.Hashtable { { "CurrentTasks", true } };
            if (string.IsNullOrEmpty(quadrant))
            {
                constraints.Add("Uncategorized", true);
                return await GetAsync(constraints);
            }
            constraints.Add("Quadrant", quadrant);
            return await GetAsync(constraints);
        }

        public override async Task<int> DeleteAsync(object id, System.Data.IDbTransaction transaction = null!, int? commandTimeout = null)
        {
            // Soft delete: set DeletedAt to DateTime.Now
            var task = await this.GetAsync(id);
            if (task == null)
                return 0;
            task.DeletedAt = System.DateTime.Now;
            return await this.UpdateAsync(task);
        }

        public async Task<int> CompleteTaskAsync(EisenhowerTask task)
        {
            if (task.IsCompleted)
                return 0;
            task.CompletedAt = System.DateTime.Now;
            return await this.UpdateAsync(task);
        }

        public async Task<int> MoveTaskToQuadrantAsync(EisenhowerTask task, string quadrant)
        {
            if (task.Quadrant == quadrant)
                return 0; // No change needed
            task.Quadrant = quadrant;
            return await this.UpdateAsync(task);
        }

        public async Task<IEnumerable<EisenhowerTask>> GetWorkedOnByDateAsync(DateTime date)
        {
            var constraints = new System.Collections.Hashtable { { "WorkedOnDate", date.Date } };
            return await GetAsync(constraints);
        }

        public async Task<IEnumerable<EisenhowerTask>> GetBlockedAsync()
        {
            var constraints = new System.Collections.Hashtable { { "Blocked", true } };
            return await GetAsync(constraints);
        }
    }
}
