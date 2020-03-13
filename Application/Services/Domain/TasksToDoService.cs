using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Domain
{
    public class TasksToDoService : ServiceBase<TasksToDo>,
                               ITasksToDoService
    {
        private readonly ITasksToDoRepository _repository;

        public TasksToDoService(ITasksToDoRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TasksToDo>> GetAllIncludingUserAsync()
        {
            return await _repository.GetAllIncludingUserAsync();
        }

        public async Task<TasksToDo> GetByIdIncludingUserAsync(int id)
        {
            return await _repository.GetByIdIncludingUserAsync(id);
        }

        public async override Task UpdateAsync(TasksToDo obj)
        {
            var TasksToDo = await GetByIdAsync(obj.Id);
            obj.Status = TasksToDo.Status;
            await base.UpdateAsync(obj);
        }
        public async Task UpdateStatusAsync(int id, bool status)
        {
            var TasksToDo = await GetByIdAsync(id);
            TasksToDo.Status = status;
            await base.UpdateAsync(TasksToDo);
        }
    }
}
