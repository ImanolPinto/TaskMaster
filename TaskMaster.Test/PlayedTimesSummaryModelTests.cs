using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using TaskMaster.Model;
using TaskMaster.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace TaskMaster.Test
{
    [TestFixture]
    public class PlayedTimesSummaryModelTests
    {
        [Test]
        public void Given_a_time_provider_when_the_played_times_summary_receives_a_null_played_task_list_then_time_items_list_is_null()
        {
            // Given
            Mock<ITimeProvider> timeProviderMock = new Mock<ITimeProvider>();
            var todayDate = new DateTime(1000, 2, 15);
            timeProviderMock.Setup(x => x.Now()).Returns(todayDate);

            // When
            var sut = new PlayedTimesSummaryModel(timeProviderMock.Object);

            // Then
            // When
            Messenger.Default.Send(new OpenPlayedTimesSummaryMsg(null));

            // Then
            Assert.IsTrue(sut.TimeListItems == null);
        }

        [Test]
        public void Given_a_time_provider_when_the_played_times_summary_receives_a_played_task_list_then_the_time_items_are_created_only_for_today()
        {
            // Given
            Mock<ITimeProvider> timeProviderMock = new Mock<ITimeProvider>();
            var todayDate = new DateTime(1000, 2, 15);
            var otherDate = new DateTime(1000, 2, 14);
            timeProviderMock.Setup(x => x.Now()).Returns(todayDate);
            var sut = new PlayedTimesSummaryModel(timeProviderMock.Object);

            var playSessionList1 = new List<PlaySession>()
            {
                new PlaySession(todayDate, new TimeSpan(1, 3, 4)),
                new PlaySession(otherDate, new TimeSpan(4, 3, 4))
            };

            var playSessionList2 = new List<PlaySession>()
            {
                new PlaySession(todayDate, new TimeSpan(3, 1, 1))
            };

            var taskItemList = new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).WithDescription("First task").WithTag("This tag").WithPlaySessions(playSessionList1).Build(),
                new TaskItemBuilder(Guid.NewGuid()).WithDescription("Other task").WithTag("This tag").WithPlaySessions(playSessionList2).Build()
            };

            sut.RegisterForMessagesBeforeView();

            // When
            Messenger.Default.Send(new OpenPlayedTimesSummaryMsg(taskItemList));

            // Then
            Assert.IsTrue(sut.TimeListItems.Count() == 2);
            Assert.IsTrue(sut.TimeListItems[0].Time == new TimeSpan(1, 3, 4));
            Assert.IsTrue(sut.TimeListItems[1].Time == new TimeSpan(3, 1, 1));
        }
    }
}
