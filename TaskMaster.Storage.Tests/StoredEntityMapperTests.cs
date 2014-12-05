using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TaskMaster.Model;
using Moq;
using System.Collections.ObjectModel;

namespace TaskMaster.Storage.Tests
{
    [TestFixture]
    public class StoredEntityMapperTests
    {
        static TaskItemBuilder _taskItemBuilder = new TaskItemBuilder();
        static PlaySessionBuilder _playSessionBuilder = new PlaySessionBuilder();

        [Test]
        public void a_Given_a_null_object_when_Map_is_requested_then_null_is_returned()
        {
            // Given
            List<TaskItemStored> taskItemStoredList = null;
            TaskItemStored taskItemStored = null;
            List<PlaySessionStored> playSessionStoredList = null;
            PlaySessionStored playSessionStored = null;

            var sut = GiveNewSutConfiguredWith(taskItemStoredList, playSessionStoredList);

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
        public void d_Given_a_populated_StoredTaskItem_list_when_Map_is_requested_then_the_task_item_collection_mapper_is_used()
        {
            // Given
            var listToMap = new List<TaskItemStored>()
            {
                new TaskItemStored() 
                { 
                    Id = Guid.NewGuid(),
                    Description = "hola",
                    Tag = "tag",
                    IsArchived = true
                },
                new TaskItemStored() 
                { 
                    Id = Guid.NewGuid()
                }
            };

            // Mock behaviour is strict, if mappers were not used it would fail
            Mock<ICollectionMapper<TaskItemStored, TaskItem>> taskItemCollectionMapperMock;
            var sut = GiveNewSutConfiguredWith(listToMap, null, out taskItemCollectionMapperMock);

            // When
            sut.Map(listToMap);
            taskItemCollectionMapperMock.VerifyAll();
        }

        [Test]
        public void e_Given_a_populated_play_session_list_when_Map_is_requested_then_the_play_session_collection_mapper_is_used()
        {
            // Mock behaviour is strict, if mappers were not used it would fail
            Mock<ICollectionMapper<PlaySessionStored, PlaySession>> playSessionCollectionMapperMock;
            var listToMap = _anyPlaySessionStoredList;
            var sut = GiveNewSutConfiguredWith(null, listToMap, out playSessionCollectionMapperMock);

            // When
            sut.Map(_anyPlaySessionStoredList);
            playSessionCollectionMapperMock.VerifyAll();
        }

        [Test]
        public void e_Given_a_complete_PlaySessionStored_when_it_is_mapped_then_it_is_mapped_correctly()
        {
            // Given
            var sut = GiveNewSutConfiguredWith(null, null);

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
            var sut = GiveNewSutConfiguredWith(null, null);
            var stored = new PlaySessionStored();

            // When
            var mapped = sut.MapPlaySessionStored(stored);
            Assert.IsTrue(mapped.Date == DateTime.MinValue);
            Assert.IsTrue(mapped.PlayedTime.Days == 0);
            Assert.IsTrue(mapped.PlayedTime.Hours == 0);
            Assert.IsTrue(mapped.PlayedTime.Minutes == 0);
            Assert.IsTrue(mapped.PlayedTime.Seconds == 0);
        }

        private static StoredEntityMapper GiveNewSutConfiguredWith(
            ICollection<TaskItemStored> storedTaskItems, 
            ICollection<PlaySessionStored> storedPlaySessions)
        {
            Mock<ICollectionMapper<TaskItemStored, TaskItem>> taskItemCollectionMapperMock;
            Mock<ICollectionMapper<PlaySessionStored, PlaySession>> playSessionCollectionMapperMock;
            var sut = GiveNewSutConfiguredWith(storedTaskItems, storedPlaySessions, out taskItemCollectionMapperMock, out playSessionCollectionMapperMock);
            return sut;
        }

        private static StoredEntityMapper GiveNewSutConfiguredWith(
            ICollection<TaskItemStored> storedTaskItems,
            ICollection<PlaySessionStored> storedPlaySessions,
            out Mock<ICollectionMapper<TaskItemStored, TaskItem>> taskItemCollectionMapperMock)
        {
            Mock<ICollectionMapper<PlaySessionStored, PlaySession>> playSessionCollectionMapperMock;
            var sut = GiveNewSutConfiguredWith(storedTaskItems, storedPlaySessions, out taskItemCollectionMapperMock, out playSessionCollectionMapperMock);
            return sut;
        }

        private static StoredEntityMapper GiveNewSutConfiguredWith(
            ICollection<TaskItemStored> storedTaskItems,
            ICollection<PlaySessionStored> storedPlaySessions,
            out Mock<ICollectionMapper<PlaySessionStored, PlaySession>> playSessionCollectionMapperMock)
        {
            Mock<ICollectionMapper<TaskItemStored, TaskItem>> taskItemCollectionMapperMock;
            var sut = GiveNewSutConfiguredWith(storedTaskItems, storedPlaySessions, out taskItemCollectionMapperMock, out playSessionCollectionMapperMock);
            return sut;
        }

        private static StoredEntityMapper GiveNewSutConfiguredWith(
            ICollection<TaskItemStored> storedTaskItems,
            ICollection<PlaySessionStored> storedPlaySessions,
            out Mock<ICollectionMapper<TaskItemStored, TaskItem>> taskItemCollectionMapperMock,
            out Mock<ICollectionMapper<PlaySessionStored, PlaySession>> playSessionCollectionMapperMock)
        {
            taskItemCollectionMapperMock = new Mock<ICollectionMapper<TaskItemStored, TaskItem>>(MockBehavior.Strict);
            playSessionCollectionMapperMock = new Mock<ICollectionMapper<PlaySessionStored, PlaySession>>(MockBehavior.Strict);
            
            var sut = new StoredEntityMapper(_taskItemBuilder, _playSessionBuilder, taskItemCollectionMapperMock.Object, playSessionCollectionMapperMock.Object);

            // setup of item mappers
            taskItemCollectionMapperMock.Setup(x => x.WithPerItemMapper(sut.MapTaskItemStored)).Returns(taskItemCollectionMapperMock.Object);
            playSessionCollectionMapperMock.Setup(x => x.WithPerItemMapper(sut.MapPlaySessionStored)).Returns(playSessionCollectionMapperMock.Object);

            taskItemCollectionMapperMock.Setup(x => x.WithCollectionToMap(storedTaskItems)).Returns(taskItemCollectionMapperMock.Object);
            taskItemCollectionMapperMock.Setup(x => x.Map()).Returns(() => new Collection<TaskItem>()); // no real mapping is performed
            playSessionCollectionMapperMock.Setup(x => x.WithCollectionToMap(storedPlaySessions)).Returns(playSessionCollectionMapperMock.Object);
            playSessionCollectionMapperMock.Setup(x => x.Map()).Returns(() => new Collection<PlaySession>()); // no real mapping is performed
            
            return sut;
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
