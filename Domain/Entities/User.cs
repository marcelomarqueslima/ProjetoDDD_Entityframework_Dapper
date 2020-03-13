using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Domain.Entities
{
    public class User : IIdentityEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private ICollection<TasksToDo> _tasksToDo { get; set; }
        public virtual IReadOnlyCollection<TasksToDo> TasksToDo { get { return _tasksToDo as Collection<TasksToDo>; } }

        public User()
        {
            this._tasksToDo = new Collection<TasksToDo>();
        }

        public void AddItemToDo(TasksToDo todo)
        {
            _tasksToDo.Add(todo);
        }
    }
}
