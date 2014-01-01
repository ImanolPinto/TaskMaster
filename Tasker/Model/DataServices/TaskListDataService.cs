using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class TaskListDataService : ITaskListDataService
    {
        public List<TaskItem> GetUnarchivedTasks()
        {
            // TODO: implement the real data access
            var taskListDesign = new Design.DesignTaskListDataService();
            return taskListDesign.GetUnarchivedTasks();
        }


        public ArchiveTaskResult ArchiveTask(TaskItem task)
        {
            // TODO: implement real data storage
            return ArchiveTaskResult.Ok;
        }
    }
}
