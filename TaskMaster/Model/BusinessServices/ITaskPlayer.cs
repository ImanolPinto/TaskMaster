using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public interface ITaskPlayer
    {
        void Play(TaskItem task);
        bool CanPlay(TaskItem _task);
        bool CanPause(TaskItem task_aux);
        void Pause(TaskItem task);
        void Reset();
    }
}
