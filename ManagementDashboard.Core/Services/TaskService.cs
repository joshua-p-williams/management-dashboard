using System;
using ManagementDashboard.Data.Models;
using ManagementDashboard.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementDashboard.Core.Services
{
    public class TaskService
    {
        private readonly IEisenhowerTaskRepository _repository;
        public TaskService(IEisenhowerTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EisenhowerTask>> GetNextTasksToWorkOnAsync(int count = 5)
        {
            var tasks = await _repository.GetOpenTasksAsync() ?? new List<EisenhowerTask>();
            return tasks
                .OrderBy(t => GetQuadrantOrder(t.Quadrant))
                .ThenByDescending(t => t.Priority)
                .ThenBy(t => t.IsBlocked)
                .ThenBy(t => t.CreatedAt)
                .Take(count)
                .ToList();
        }

        private int GetQuadrantOrder(string? quadrant)
        {
            if (Enum.TryParse<EisenhowerQuadrant>(quadrant, out var q))
                return (int)q;
            return 5;
        }
    }
}
