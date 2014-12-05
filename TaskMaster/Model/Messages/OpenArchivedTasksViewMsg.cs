using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class OpenArchivedTasksViewMsg
    {
        ICollection<TaskItem> _taskItems;

        public ICollection<TaskItem> TaskItems
        {
            get { return _taskItems; }
        }

        public OpenArchivedTasksViewMsg(ICollection<TaskItem> taskItems)
        {
            _taskItems = taskItems;
        }
    }
}
