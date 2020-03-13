using Domain.Entities;
using System;
using System.Collections.Generic;

namespace UnitTest.Integration.Repositories.Repositories.DataBuilder
{
    public class TasksToDoBuilder
    {
        private TasksToDo TasksToDo;
        private List<TasksToDo> TasksToDoList;

        public TasksToDoBuilder()
        {
        }

        public TasksToDo CreateTasksToDo()
        {
            TasksToDo = new TasksToDo() { Title = "Task from Builder", Start = DateTime.Now, DeadLine = DateTime.Now };
            return TasksToDo;
        }

        public TasksToDo CreateTasksToDoWithUser(int id)
        {
            TasksToDo = new TasksToDo() { Title = "Task from Builder", Start = DateTime.Now, DeadLine = DateTime.Now, UserId = id };
            return TasksToDo;
        }

        public List<TasksToDo> CreateTasksToDoList(int amount)
        {
            TasksToDoList = new List<TasksToDo>();
            for (int i = 0; i < amount; i++)
            {
                TasksToDoList.Add(CreateTasksToDo());
            }

            return TasksToDoList;
        }

        public List<TasksToDo> CreateTasksToDoListWithUser(int amount, int id)
        {
            TasksToDoList = new List<TasksToDo>();
            for (int i = 0; i < amount; i++)
            {
                TasksToDoList.Add(CreateTasksToDoWithUser(id));
            }

            return TasksToDoList;
        }
    }
}