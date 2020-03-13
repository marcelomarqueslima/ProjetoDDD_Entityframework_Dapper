using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Application.Interfaces.Services.Domain;

namespace WebApplication.Controllers
{
    public class TasksToDoController : Controller
    {
        private readonly ITasksToDoService _tasksToDoService;

        public TasksToDoController(ITasksToDoService tasksToDo)
        {
            _tasksToDoService = tasksToDo;
        }

        // GET: TasksToDo
        public async Task<IActionResult> Index()
        {
            return View(await _tasksToDoService.GetAllAsync());
        }

        // GET: TasksToDo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksToDo = await _tasksToDoService.GetByIdAsync(id);
            if (tasksToDo == null)
            {
                return NotFound();
            }

            return View(tasksToDo);
        }

        // GET: TasksToDo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TasksToDo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Start,DeadLine,Status,UserId")] TasksToDo tasksToDo)
        {
            if (ModelState.IsValid)
            {
                await _tasksToDoService.AddAsync(tasksToDo);
                return RedirectToAction(nameof(Index));
            }
            return View(tasksToDo);
        }

        // GET: TasksToDo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksToDo = await _tasksToDoService.GetByIdAsync(id);
            if (tasksToDo == null)
            {
                return NotFound();
            }
            return View(tasksToDo);
        }

        // POST: TasksToDo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Start,DeadLine,Status,UserId")] TasksToDo tasksToDo)
        {
            if (id != tasksToDo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tasksToDoService.UpdateAsync(tasksToDo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksToDoExists(tasksToDo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tasksToDo);
        }

        // GET: TasksToDo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksToDo = await _tasksToDoService.GetByIdAsync(id);
            if (tasksToDo == null)
            {
                return NotFound();
            }


            return View(tasksToDo);
        }

        // POST: TasksToDo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tasksToDoService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TasksToDoExists(int id)
        {
            return _tasksToDoService.GetByIdAsync(id) != null ? true : false;
        }
    }
}
