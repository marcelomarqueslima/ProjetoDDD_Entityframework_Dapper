using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Domain.Dapper;
using NUnit.Framework;
using System.Threading.Tasks;
using UnitTest.Integration.Repositories.Repositories.DataBuilder;
using System.Linq;
using UnitTest.Integration.Repositories.DBConfiguration.Dapper;
using System.Transactions;
using Infrastructure.Interfaces.DBConfiguration;

namespace UnitTest.Integration.Repositories.Repositories.Dapper
{
    [TestFixture]
    public class TaskToDoRepositoryTransactionScopeTest
    {
        private IDatabaseFactory databaseOptions;

        private IUserRepository userDapper;
        private ITasksToDoRepository taskToDoDapper;
        private UserBuilder userBuilder;
        private TasksToDoBuilder taskToDoBuilder;

        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            databaseOptions = new DapperConnection().DatabaseFactory();
        }

        [SetUp]
        public void Inicializa()
        {
            userBuilder = new UserBuilder();
            taskToDoBuilder = new TasksToDoBuilder();
        }

        [TearDown]
        public void ExecutadoAposExecucaoDeCadaTeste()
        {
        }

        [Test]
        public async Task GetAllIncludingUserAsync()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var conn = databaseOptions.GetDbConnection;
                userDapper = new UserRepository(conn);
                taskToDoDapper = new TasksToDoRepository(conn);

                var user = await userDapper.AddAsync(userBuilder.CreateUser());
                var task = await taskToDoDapper.AddRangeAsync(taskToDoBuilder.CreateTasksToDoListWithUser(2, user.Id));

                var result = await taskToDoDapper.GetAllIncludingUserAsync();

                Assert.AreEqual(result.FirstOrDefault().UserId, user.Id);
                Assert.AreEqual(result.LastOrDefault().UserId, user.Id);
            }
        }

        [Test]
        public async Task GetByIdIncludingUserAsync()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var conn = databaseOptions.GetDbConnection;
                userDapper = new UserRepository(conn);
                taskToDoDapper = new TasksToDoRepository(conn);

                var user = await userDapper.AddAsync(userBuilder.CreateUser());
                var task = await taskToDoDapper.AddAsync(taskToDoBuilder.CreateTasksToDoWithUser(user.Id));

                var result = await taskToDoDapper.GetByIdIncludingUserAsync(task.Id);

                Assert.AreEqual(result.UserId, user.Id);
            }
        }
    }
}