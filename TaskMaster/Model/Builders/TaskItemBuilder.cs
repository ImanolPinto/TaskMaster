﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class TaskItemBuilder
    {
        Guid _taskId;
        string _taskDescription;
        string _taskTag;

        public TaskItemBuilder(Guid taskId)
        {
            _taskId = taskId;
        }

        public TaskItemBuilder WithDescription(string description)
        {
            _taskDescription = description;
            return this;
        }

        public TaskItemBuilder WithTag(string tag)
        {
            _taskTag = tag;
            return this;
        }

        public TaskItem Build()
        {
            var task = new TaskItem(_taskId, _taskDescription);
            task.Tag = _taskTag;
            return task;
        }
    }
}