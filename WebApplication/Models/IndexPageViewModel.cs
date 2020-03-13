using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class IndexPageViewModel
    {
        public int? UserId { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<TasksToDo> TasksFromUser { get; set; }

        public IndexPageViewModel()
        {
            Users = new List<User>();
            TasksFromUser = new List<TasksToDo>();
        }

        public IndexPageViewModel MapUsersToViewModel(int? id, IEnumerable<User> users)
        {
            this.Users = users.ToList();
            this.UserId = id;

            if (id != null)
            {
                var tasks = Users.Single(u => u.Id == id.Value)?.TasksToDo;
                if (tasks.FirstOrDefault() != null)
                {
                    this.TasksFromUser = tasks;
                }
            }

            return this;
        }
    }
}
