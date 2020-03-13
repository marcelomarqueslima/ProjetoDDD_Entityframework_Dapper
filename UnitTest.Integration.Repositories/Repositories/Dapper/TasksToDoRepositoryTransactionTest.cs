using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Domain.Dapper;
using NUnit.Framework;
using System.Threading.Tasks;
using UnitTest.Integration.Repositories.Repositories.DataBuilder;
using System.Linq;
using System.Data;
using UnitTest.Integration.Repositories.DBConfiguration.Dapper;
using Infrastructure.Interfaces.DBConfiguration;

namespace UnitTest.Integration.Repositories.Repositories.Dapper
{
    [TestFixture]
    public class TasksToDoRepositoryTransactionTest
    {
        private IDatabaseFactory databaseOptions;
        private IDbTransaction transaction;

        private IUserRepository userDapper;
        private ITasksToDoRepository TasksToDoDapper;
        private UserBuilder userBuilder;
        private TasksToDoBuilder TasksToDoBuilder;

        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            databaseOptions = new DapperConnection().DatabaseFactory();
        }

        [SetUp]
        public void Inicializa()
        {
            userBuilder = new UserBuilder();
            TasksToDoBuilder = new TasksToDoBuilder();
            var conn = databaseOptions.GetDbConnection;
            conn.Open();
            transaction = conn.BeginTransaction();
            userDapper = new UserRepository(conn, transaction);
            TasksToDoDapper = new TasksToDoRepository(conn, transaction);
        }

        [TearDown]
        public void ExecutadoAposExecucaoDeCadaTeste()
        {
            transaction.Rollback();
        }

        [Test]
        public async Task GetAllIncludingUserAsync()
        {
            var user = await userDapper.AddAsync(userBuilder.CreateUser());
            var task = await TasksToDoDapper.AddRangeAsync(TasksToDoBuilder.CreateTasksToDoListWithUser(2, user.Id));

            var result = await TasksToDoDapper.GetAllIncludingUserAsync();

            Assert.AreEqual(result.FirstOrDefault().UserId, user.Id);
            Assert.AreEqual(result.LastOrDefault().UserId, user.Id);
        }

        [Test]
        public async Task GetByIdIncludingUserAsync()
        {
            var user = await userDapper.AddAsync(userBuilder.CreateUser());
            var task = await TasksToDoDapper.AddAsync(TasksToDoBuilder.CreateTasksToDoWithUser(user.Id));

            var result = await TasksToDoDapper.GetByIdIncludingUserAsync(task.Id);

            Assert.AreEqual(result.UserId, user.Id);
        }
    }
}