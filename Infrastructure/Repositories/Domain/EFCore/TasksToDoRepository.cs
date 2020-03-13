using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class TasksToDoRepository : DomainRepository<TasksToDo>,
                                  ITasksToDoRepository
    {
        public TasksToDoRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<TasksToDo>> GetAllIncludingUserAsync()
        {
            IQueryable<TasksToDo> query = await Task.FromResult(GenerateQuery(null, null, nameof(TasksToDo.User)));
            return query.AsEnumerable();
        }

        public async Task<TasksToDo> GetByIdIncludingUserAsync(int id)
        {
            IQueryable<TasksToDo> query = await Task.FromResult(GenerateQuery((taskToDo => taskToDo.Id == id), null, nameof(TasksToDo.User)));
            return query.SingleOrDefault();
        }
    }
}
