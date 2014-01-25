using System;
using System.Collections.Generic;
using TaskMaster.Model;

namespace TaskMaster.Design
{
    public class DesignTaskListDataService : ITaskListDataService
    {
        public List<TaskItem> GetActiveTasks()
        {
            var list = new List<TaskItem>();

            list.Add(new TaskItemBuilder(Guid.NewGuid())
                .WithDescription("Esta descripción es muy larga. Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga Larga")
                .WithTag("34535")
                .Build());

            for(int i=0; i < 30; i++)
            {
                list.Add(new TaskItemBuilder(Guid.NewGuid())
                    .WithDescription("Descripción Tarea " + i)
                    .WithTag(i.ToString())
                    .Build());
            }

            return list;
        }

        public List<TaskItem> GetRecentArchivedTasks()
        {
            var list = new List<TaskItem>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TaskItemBuilder(Guid.NewGuid())
                    .WithDescription("Archived task description " + i)
                    .WithTag(i.ToString())
                    .Build());
            }

            return list;
        }

        public ArchiveTaskResult ArchiveTask(TaskItem task)
        {
            throw new NotImplementedException();
        }
    }
}