using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMaster.Model;

namespace TaskMaster.Storage
{
    public interface IStoredEntityMapper
    {
        ICollection<TaskItem> Map(ICollection<TaskItemStored> storedList);
    }
}
