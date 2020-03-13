using Application.Interfaces.Services.Standard;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.Domain
{
    public interface ITasksToDoService : IServiceBase<TasksToDo>
    {
        Task UpdateStatusAsync(int id, bool status);
        Task<IEnumerable<TasksToDo>> GetAllIncludingUserAsync();
        Task<TasksToDo> GetByIdIncludingUserAsync(int id);
    }
}
