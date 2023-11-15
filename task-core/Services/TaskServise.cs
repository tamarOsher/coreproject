using Models;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace Services
{
    public class TaskServise : ITaskService
    {

        List<Task> Tasks { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskServise(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }

        public List<Task> GetAll(long userId){
            return Tasks.Where(p => p.AgentId == userId).ToList();

        }
        public Task GetById(long userId,int id){
           return Tasks.FirstOrDefault(t => t.AgentId==userId &&t.Id==id);
        }
        public void Add(long userId,Task task)
        {
            task.Id = Tasks.Count() + 1;
            task.AgentId=userId;
            Tasks.Add(task);
            saveToFile();
        }
    
   

    
        public void Update(long userId,Task task)
        {
            var index = Tasks.FindIndex(p => p.AgentId==userId&&p.Id == task.Id);
            if (index == -1)
                return;

            Tasks[index] = task;
            saveToFile();
        }

    
        public void Delete(long userId,int id)
        {
            var task = GetById(userId,id);
            if (task is null)
                return;

            Tasks.Remove(task);
            saveToFile();
        }

            public int Count(long userId){
                return GetAll(userId).Count();
            } 
    }

    
}