using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Controllers{

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "Agent")]

public class TasksController : ControllerBase
{
    private long userId;
    private ITaskService TaskServise;


    public TasksController(ITaskService TaskServise)
    {
        this.TaskServise = TaskServise;
        this.userId = long.Parse(User.FindFirst("UserId")?.Value ?? "");
    }

    [HttpGet]
    public ActionResult<List<Task>> GetAll() =>
        TaskServise.GetAll(userId);


    [HttpGet("{id}")]
        public ActionResult<Task> GetById(long userId,int id)
        {
            var task = TaskServise.GetById(userId,id);

            if (task == null)
                return NotFound();

            return task;
        }

    [HttpPost]
        public IActionResult Create(Task task)
        {
            TaskServise.Add(userId,task);
            return CreatedAtAction(nameof(Create), new {id=task.Id}, task);

        }

    [HttpPut("{id}")]
        public IActionResult Update(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest();

            var MyTask = TaskServise.GetById(userId,id);
            if (MyTask is null)
                return  NotFound();

            TaskServise.Update(userId,task);

            return NoContent();
        } 

    [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TaskServise.GetById(userId,id);
            if (task is null)
                return  NotFound();

            TaskServise.Delete(userId,id);

            return Content(TaskServise.Count(userId).ToString());
        }
    
   } 
    
}

