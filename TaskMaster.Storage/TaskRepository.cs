using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using TaskMaster.Model;

namespace TaskMaster.Storage
{
    public class TaskRepository : ITaskListDataService
    {
        IStoredEntityMapper _mapper;
        TaskMasterSQLEntities _sqlEntities;
        public TaskRepository(IStoredEntityMapper mapper)
        {
            Ensure.IsNotNull("IEntityMapper", mapper);

            _mapper = mapper;
            _sqlEntities = new TaskMasterSQLEntities();
        }

        public ICollection<TaskItem> GetActiveTasks()
        {
            var storedList = _sqlEntities.TaskItemStoreds.Where(x => x.IsArchived == false).ToList();
            return _mapper.Map(storedList);
        }

        public ICollection<TaskItem> GetRecentArchivedTasks()
        {
            throw new NotImplementedException();
        }

        public ArchiveTaskResult ArchiveTask(TaskItem task)
        {
            throw new NotImplementedException();
        }
    }
}
