using System;
using System.Collections.Generic;
using TaskMaster.Model;

namespace TaskMaster.Design
{
    public class DesignTaskListDataService : ITaskListDataService
    {
        public List<TaskItem> GetUnarchivedTasks()
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


        public ArchiveTaskResult ArchiveTask(TaskItem task)
        {
            throw new NotImplementedException();
        }
    }
}