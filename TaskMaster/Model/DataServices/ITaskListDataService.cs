﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Model;

namespace TaskMaster.Model
{
    public interface ITaskListDataService
    {
        ICollection<TaskItem> GetActiveTasks();
        ICollection<TaskItem> GetRecentArchivedTasks();

        ArchiveTaskResult ArchiveTask(TaskItem task);
    }
}
