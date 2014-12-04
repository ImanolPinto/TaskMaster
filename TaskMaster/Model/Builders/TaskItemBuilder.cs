using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class TaskItemBuilder : ITaskItemBuilder
    {
        Guid _taskId;
        string _taskDescription;
        string _taskTag;
        List<PlaySession> _playSessions;

        public TaskItemBuilder()
        {
        }

        public TaskItemBuilder(Guid taskId)
        {
            _taskId = taskId;
        }

        public ITaskItemBuilder WithTaskId(Guid taskId)
        {
            _taskId = taskId;
            return this;
        }

        public ITaskItemBuilder WithDescription(string description)
        {
            _taskDescription = description;
            return this;
        }

        public ITaskItemBuilder WithTag(string tag)
        {
            _taskTag = tag;
            return this;
        }

        public ITaskItemBuilder WithPlaySessions(List<PlaySession> playSessions)
        {
            _playSessions = playSessions;
            return this;
        }

        public TaskItem Build()
        {
            var task = new TaskItem(_taskId, _taskDescription);
            task.Tag = _taskTag;
            task.LoadPlaySessions(_playSessions);
            return task;
        }

        public ITaskItemBuilder Reset()
        {
            return new TaskItemBuilder();
        }
    }
}
