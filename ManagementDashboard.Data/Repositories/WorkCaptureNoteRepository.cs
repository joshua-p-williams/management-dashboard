using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Formula.SimpleRepo;
using ManagementDashboard.Data.Models;
using Microsoft.Extensions.Configuration;

namespace ManagementDashboard.Data.Repositories
{
    public interface IWorkCaptureNoteRepository : IRepository<WorkCaptureNote>
    {
        Task<IEnumerable<WorkCaptureNote>> GetNotesByDateAsync(System.DateTime date);
        Task<IEnumerable<WorkCaptureNote>> GetNotesByTaskIdAsync(int taskId);
    }

    public class WorkCaptureNoteRepository : RepositoryBase<WorkCaptureNote, WorkCaptureNoteConstraints>, IWorkCaptureNoteRepository
    {
        public WorkCaptureNoteRepository(IConfiguration config) : base(config) { }

        public async Task<IEnumerable<WorkCaptureNote>> GetNotesByDateAsync(System.DateTime date)
        {
            var constraints = new Hashtable { { "CreatedOnDate", date.Date } };
            // This assumes a constraint or query for date only; may need custom constraint for date part only
            return await GetAsync(constraints);
        }

        public async Task<IEnumerable<WorkCaptureNote>> GetNotesByTaskIdAsync(int taskId)
        {
            var constraints = new Hashtable { { "TaskId", taskId } };
            return await GetAsync(constraints);
        }
    }
}
