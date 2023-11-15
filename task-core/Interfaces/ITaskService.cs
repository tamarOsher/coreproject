using Models;
using System.Collections.Generic;


namespace Interfaces
{
    public interface ITaskService
    {
        List<Task> GetAll(long userId);
        Task GetById(long userId,int id);
        void Add(long userId,Task task);
        void Delete(long userId,int id);
        void Update(long userId,Task task);
        int Count(long userId);
    }
}