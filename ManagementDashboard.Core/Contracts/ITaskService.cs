using ManagementDashboard.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementDashboard.Core.Contracts
{
    public interface ITaskService
    {
        Task<List<EisenhowerTask>> GetNextTasksToWorkOnAsync(int? count = 5);
    }
}
