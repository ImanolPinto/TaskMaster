using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TaskMaster.Model;

namespace TaskMaster.Storage.Tests
{
    [TestFixture]
    public class StoredEntityMapperTests
    {
        private StoredEntityMapper sut;
        
        [TestFixtureSetUp]
        public void Setup()
        {
            var _taskItemBuilder = new TaskItemBuilder();
            var _playSessionBuilder = new PlaySessionBuilder();
            var _taskItemCollectionMapper = new CollectionMapper<TaskItemStored, TaskItem>();
            var _playSessionCollectionMapper = new CollectionMapper<PlaySessionStored, PlaySession>();
            sut = new StoredEntityMapper(_taskItemBuilder, _playSessionBuilder, _taskItemCollectionMapper, _playSessionCollectionMapper);
        }

        [Test]
        public void a_Given_a_null_object_when_Map_is_requested_then_null_is_returned()
        {
            // Given
            List<TaskItemStored> taskItemStoredList = null;
            TaskItemStored taskItemStored = null;
            List<PlaySessionStored> playSessionStoredList = null;
            PlaySessionStored playSessionStored = null;

            // When
            var taskItemStoredListResult = sut.Map(taskItemStoredList);
            var taskItemStoredResult = sut.MapTaskItemStored(taskItemStored);
            var playSessionStoredListResult = sut.Map(playSessionStoredList);
            var playSessionStoredResult = sut.MapPlaySessionStored(playSessionStored);

            // Then
            Assert.IsTrue(taskItemStoredListResult == null);
            Assert.IsTrue(taskItemStoredResult == null);
            Assert.IsTrue(playSessionStoredListResult == null);
            Assert.IsTrue(playSessionStoredResult == null);
        }

        
        
        [Test]
        public void d_Given_a_populated_StoredTaskItem_list_when_Map_is_requested_then_it_is_mapped_correctly()
        {
            // Given
            var listToMap = new List<TaskItemStored>()
            {
                new TaskItemStored() 
                { 
                    Id = Guid.NewGuid(),
                    Description = "hola",
                    Tag = "tag",
                    IsArchived = true,
                    PlaySessions = _anyPlaySessionStoredList
                },
                new TaskItemStored() 
                { 
                    Id = Guid.NewGuid()
                }
            };

            // When
            List<TaskItem> mappedItems = sut.Map(listToMap).ToList();

            // Then
            Assert.IsTrue(mappedItems.Count == listToMap.Count);
            Assert.IsTrue(mappedItems[0].PlaySessions.Count == listToMap[0].PlaySessions.Count);
            Assert.IsTrue(mappedItems[0].Id == listToMap[0].Id);
        }

        [Test]
        public void e_Given_a_complete_PlaySessionStored_when_it_is_mapped_then_it_is_mapped_correctly()
        {
            // When
            var mapped = sut.MapPlaySessionStored(_completePlaySessionStored);

            // Then
            Assert.IsTrue(mapped.Date == new DateTime(2000, 1, 1));
            Assert.IsTrue(mapped.PlayedTime.Days == 2);
            Assert.IsTrue(mapped.PlayedTime.Hours == 4);
            Assert.IsTrue(mapped.PlayedTime.Minutes == 1);
            Assert.IsTrue(mapped.PlayedTime.Seconds == 7);
        }

        [Test]
        public void f_Given_a_PlaySessionStored_with_not_initialized_properties_when_it_is_mapped_then_it_is_mapped_correctly()
        {
            // Given
            var stored = new PlaySessionStored();

            // When
            var mapped = sut.MapPlaySessionStored(stored);
            Assert.IsTrue(mapped.Date == DateTime.MinValue);
            Assert.IsTrue(mapped.PlayedTime.Days == 0);
            Assert.IsTrue(mapped.PlayedTime.Hours == 0);
            Assert.IsTrue(mapped.PlayedTime.Minutes == 0);
            Assert.IsTrue(mapped.PlayedTime.Seconds == 0);
        }

        

        private static PlaySessionStored _completePlaySessionStored = new PlaySessionStored()
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2000,1,1),
            PlayedTime_Days = 2,
            PlayedTime_Hours = 4,
            PlayedTime_Minutes = 1,
            PlayedTime_Seconds = 7
        };


        private static ICollection<PlaySessionStored> _anyPlaySessionStoredList = new List<PlaySessionStored>()
        {
            _completePlaySessionStored,
            new PlaySessionStored()
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2014,1,1),
                PlayedTime_Hours = 2,
            }};
    }
}
