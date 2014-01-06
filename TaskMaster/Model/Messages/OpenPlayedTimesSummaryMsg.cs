using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class OpenPlayedTimesSummaryMsg
    {
        List<TaskItem> _taskItems;

        public List<TaskItem> TaskItems
        {
            get { return _taskItems; }
        }

        public OpenPlayedTimesSummaryMsg(List<TaskItem> taskItems)
        {
            _taskItems = taskItems;
        }
    }
}
