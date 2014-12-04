using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Model;

namespace TaskMaster.Storage
{
    public class StoredEntityMapper : IStoredEntityMapper
    {
        ITaskItemBuilder _taskItemBuilder;
        IPlaySessionBuilder _playSessionBuilder;
        ICollectionMapper<TaskItemStored, TaskItem> _taskItemStoredCollectionMapper;
        ICollectionMapper<PlaySessionStored, PlaySession> _playSessionStoredCollectionMapper;

        public StoredEntityMapper
            (ITaskItemBuilder taskItemBuilder, 
            IPlaySessionBuilder playSessionBuilder, 
            ICollectionMapper<TaskItemStored, TaskItem> taskItemStoredCollectionMapper,
            ICollectionMapper<PlaySessionStored, PlaySession> playSessionStoredCollectionMapper)
        {
            Ensure.IsNotNull("ITaskItemBuilder", taskItemBuilder);
            Ensure.IsNotNull("IPlaySessionBuilder", playSessionBuilder);
            Ensure.IsNotNull("ICollectionMapper<TaskItemStored, TaskItem>", taskItemStoredCollectionMapper);
            Ensure.IsNotNull("ICollectionMapper<PlaySessionStored, PlaySession>", playSessionStoredCollectionMapper);

            _taskItemBuilder = taskItemBuilder;
            _playSessionBuilder = playSessionBuilder;
            _taskItemStoredCollectionMapper = taskItemStoredCollectionMapper;
            _playSessionStoredCollectionMapper = playSessionStoredCollectionMapper;
        }

        public ICollection<TaskItem> Map(ICollection<TaskItemStored> storedList)
        {
            if (storedList == null)
                return null;

            return _taskItemStoredCollectionMapper
                .WithPerItemMapper(this.MapTaskItemStored)
                .WithCollectionToMap(storedList)
                .Map().ToList();
        }

        public TaskItem MapTaskItemStored(TaskItemStored storedItem)
        {
            if (storedItem == null)
                return null;

            var taskItem =
                _taskItemBuilder.WithTaskId(storedItem.Id)
                .WithDescription(storedItem.Description)
                .WithPlaySessions(Map(storedItem.PlaySessions).ToList())
                .WithTag(storedItem.Tag)
                .Build();

            return taskItem;
        }

        public ICollection<PlaySession> Map(ICollection<PlaySessionStored> playSessionStoredCollection)
        {
            if (playSessionStoredCollection == null)
                return null;

            return _playSessionStoredCollectionMapper
                .WithPerItemMapper(this.MapPlaySessionStored)
                .WithCollectionToMap(playSessionStoredCollection)
                .Map();
        }

        public PlaySession MapPlaySessionStored(PlaySessionStored playSessionStored)
        {
            if (playSessionStored == null)
                return null;

            var timeSpan = new TimeSpan(
                playSessionStored.PlayedTime_Days, 
                playSessionStored.PlayedTime_Hours, 
                playSessionStored.PlayedTime_Minutes, 
                playSessionStored.PlayedTime_Seconds);

            var mapped = _playSessionBuilder
                .WithDate(playSessionStored.Date)
                .WithPlayedTime(timeSpan)
                .Build();

            return mapped;
        }
    }
}
