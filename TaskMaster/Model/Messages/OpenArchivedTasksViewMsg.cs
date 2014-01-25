using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class OpenArchivedTasksViewMsg
    {
        List<TaskItem> _taskItems;

        public List<TaskItem> TaskItems
        {
            get { return _taskItems; }
        }

        public OpenArchivedTasksViewMsg(List<TaskItem> taskItems)
        {
            _taskItems = taskItems;
        }
    }
}
