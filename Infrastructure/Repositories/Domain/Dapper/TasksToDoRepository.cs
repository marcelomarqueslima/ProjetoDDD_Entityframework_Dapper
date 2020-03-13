using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces.DBConfiguration;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.Dapper;

namespace Infrastructure.Repositories.Domain.Dapper
{
    public class TasksToDoRepository : DomainRepository<TasksToDo>,
                                  ITasksToDoRepository
    {
        public TasksToDoRepository(IDatabaseFactory databaseOptions) : base(databaseOptions)
        {
        }

        public TasksToDoRepository(IDbConnection databaseConnection, IDbTransaction transaction = null) : base(databaseConnection, transaction)
        {
        }

        protected override string InsertQuery => $"INSERT INTO [{nameof(TasksToDo)}] VALUES (@{nameof(TasksToDo.Title)}, @{nameof(TasksToDo.Start)}, @{nameof(TasksToDo.DeadLine)}, @{nameof(TasksToDo.Status)}, @{nameof(TasksToDo.UserId)})";
        protected override string InsertQueryReturnInserted => $"INSERT INTO [{nameof(TasksToDo)}] OUTPUT INSERTED.* VALUES (@{nameof(TasksToDo.Title)}, @{nameof(TasksToDo.Start)}, @{nameof(TasksToDo.DeadLine)}, @{nameof(TasksToDo.Status)}, @{nameof(TasksToDo.UserId)})";
        protected override string UpdateByIdQuery => $"UPDATE [{nameof(TasksToDo)}] SET {nameof(TasksToDo.Title)} = @{nameof(TasksToDo.Title)}, {nameof(TasksToDo.Start)} = @{nameof(TasksToDo.Start)}, {nameof(TasksToDo.DeadLine)} = @{nameof(TasksToDo.DeadLine)}, {nameof(TasksToDo.Status)} = @{nameof(TasksToDo.Status)} WHERE {nameof(TasksToDo.Id)} = @{nameof(TasksToDo.Id)}";
        protected override string DeleteByIdQuery => $"DELETE FROM [{nameof(TasksToDo)}] WHERE {nameof(TasksToDo.Id)} = @{nameof(TasksToDo.Id)}";
        protected override string SelectAllQuery => $"SELECT * FROM [{nameof(TasksToDo)}]";
        protected override string SelectByIdQuery => $"SELECT t.* FROM [{nameof(TasksToDo)}] t WHERE t.{nameof(TasksToDo.Id)} = @{nameof(TasksToDo.Id)}";

        private string SelectAllIncludingRelation => $"SELECT t.*, u.* FROM [{nameof(TasksToDo)}] t INNER JOIN [{nameof(User)}] u ON u.{nameof(User.Id)} = t.{nameof(TasksToDo.UserId)}";
        private string SelectByIdIncludingRelation => $"SELECT t.*, u.* FROM [{nameof(TasksToDo)}] t INNER JOIN [{nameof(User)}] u ON u.{nameof(User.Id)} = t.{nameof(TasksToDo.UserId)} WHERE t.{nameof(TasksToDo.Id)} = @{nameof(TasksToDo.Id)}";

        public async Task<IEnumerable<TasksToDo>> GetAllIncludingUserAsync()
        {
            var queryResult = await dbConn.QueryAsync<TasksToDo, User, TasksToDo>(SelectAllIncludingRelation, transaction: dbTransaction,
                map: (TasksToDo, user) => FuncMapRelation(TasksToDo, user));

            return queryResult.Distinct();
        }

        public async Task<TasksToDo> GetByIdIncludingUserAsync(int id)
        {
            var queryResult = await dbConn.QueryAsync<TasksToDo, User, TasksToDo>(SelectByIdIncludingRelation, param: new { Id = id }, transaction: dbTransaction,
                map: (TasksToDo, user) => FuncMapRelation(TasksToDo, user));

            return queryResult.Distinct().FirstOrDefault();
        }

        private readonly Func<TasksToDo, User, TasksToDo> FuncMapRelation = (tasksToDo, user) =>
        {
            tasksToDo.User = user;
            return tasksToDo;
        };
    }
}
