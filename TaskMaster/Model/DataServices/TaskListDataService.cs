using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class TaskListDataService : ITaskListDataService
    {
        public ICollection<TaskItem> GetActiveTasks()
        {
            // TODO: implement the real data access
            var taskListDesign = new Design.DesignTaskListDataService();
            return taskListDesign.GetActiveTasks();
        }


        public ArchiveTaskResult ArchiveTask(TaskItem task)
        {
            // TODO: implement real data storage
            return ArchiveTaskResult.Ok;
        }


        public ICollection<TaskItem> GetRecentArchivedTasks()
        {
            // TODO: implement real data access
            var taskListDesign = new Design.DesignTaskListDataService();
            return taskListDesign.GetRecentArchivedTasks();
        }
    }
}
