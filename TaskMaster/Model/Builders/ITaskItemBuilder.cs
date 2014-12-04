using System;
using System.Collections.Generic;
namespace TaskMaster.Model
{
    public interface ITaskItemBuilder
    {
        ITaskItemBuilder WithTaskId(Guid guid);
        ITaskItemBuilder WithDescription(string description);
        ITaskItemBuilder WithPlaySessions(List<PlaySession> playSessions);
        ITaskItemBuilder WithTag(string tag);
        TaskItem Build();
        ITaskItemBuilder Reset();
    }
}
