using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public enum PlayingState
    {
        None = -1,
        Playing = 0,
        Stopped = 1,
        Paused = 2,
        Completed = 3
    }
}
