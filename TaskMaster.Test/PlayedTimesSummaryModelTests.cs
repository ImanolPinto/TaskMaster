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
            var sut = new PlayedTimesSummaryModel(timeProviderMock.Object);

            // When
            Messenger.Default.Send(new OpenPlayedTimesSummaryMsg(null));

            // Then
            Assert.IsTrue(sut.TimeListItems == null);
        }

        [Test]
        public void Given_a_time_provider_when_the_played_times_summary_receives_a_played_task_list_then_the_time_items_are_created_only_for_today()
        {
            // Given
            var targetDate = new DateTime(1000, 2, 15);
            var otherDate = new DateTime(1000, 2, 14);
            var sut = SutWithTimeProviderReturningATargetDate(targetDate);
            var taskItemList = TaskItemListWithTasksPlayedTheSameTargetDate(targetDate, otherDate);
            sut.RegisterForMessagesBeforeView();

            // When
            Messenger.Default.Send(new OpenPlayedTimesSummaryMsg(taskItemList));

            // Then
            Assert.IsTrue(OnlyTasksItemsForTheTargetDateAreInTheTimeListItemList(sut.TimeListItems.ToList()));
        }

        [Test]
        public void Given_a_time_provider_when_the_played_times_summary_receives_a_played_task_list_with_a_played_time_less_than_a_minute_for_the_target_date_then_no_time_items_are_created()
        {
            // Given
            var targetDate = new DateTime(1000, 2, 15);
            var sut = SutWithTimeProviderReturningATargetDate(targetDate);
            var taskItemList = TaskItemListWithPlayedTimeLessThanAMinuteOnTheTargetDate(targetDate);
            sut.RegisterForMessagesBeforeView();

            // When
            Messenger.Default.Send(new OpenPlayedTimesSummaryMsg(taskItemList));

            // Then
            Assert.IsTrue(sut.TimeListItems.Count == 0);
        }

        [Test]
        public void When_the_target_date_changes_then_the_task_item_list_is_refreshed_for_that_date()
        {
            // Given
            var initialTargetDate = new DateTime(2000, 1, 1);
            var newTargetDate = new DateTime(2014, 1, 1);
            var sut = SutWithTimeProviderReturningATargetDate(initialTargetDate);

            // When
            sut.TargetDate = newTargetDate;

            // Then

        }

        #region Helpers

        private static PlayedTimesSummaryModel SutWithTimeProviderReturningATargetDate(DateTime targetDate)
        {
            Mock<ITimeProvider> timeProviderMock = new Mock<ITimeProvider>();
            timeProviderMock.Setup(x => x.Now()).Returns(targetDate);
            var sut = new PlayedTimesSummaryModel(timeProviderMock.Object);
            return sut;
        }

        private static List<TaskItem> TaskItemListWithTasksPlayedTheSameTargetDate(DateTime targetDate, DateTime otherDate)
        {
            var playSessionList1 = new List<PlaySession>()
            {
                new PlaySession(targetDate, new TimeSpan(1, 3, 4)),
                new PlaySession(otherDate, new TimeSpan(4, 3, 4))
            };

            var playSessionList2 = new List<PlaySession>()
            {
                new PlaySession(targetDate, new TimeSpan(3, 1, 1))
            };

            var taskItemList = new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).WithDescription("First task").WithTag("This tag").WithPlaySessions(playSessionList1).Build(),
                new TaskItemBuilder(Guid.NewGuid()).WithDescription("Other task").WithTag("This tag").WithPlaySessions(playSessionList2).Build()
            };
            return taskItemList;
        }

        private bool OnlyTasksItemsForTheTargetDateAreInTheTimeListItemList(List<TimeItem> timeItemList)
        {
            return timeItemList.Count == 2
                && timeItemList[0].Time == new TimeSpan(1, 3, 4)
                && timeItemList[1].Time == new TimeSpan(3, 1, 1);
        }

        private List<TaskItem> TaskItemListWithPlayedTimeLessThanAMinuteOnTheTargetDate(DateTime targetDate)
        {
            var playSessionList1 = new List<PlaySession>()
            {
                new PlaySession(targetDate, new TimeSpan(0, 0, 59)),
            };

            var taskItemList = new List<TaskItem>()
            {
                new TaskItemBuilder(Guid.NewGuid()).WithDescription("First task").WithTag("This tag").WithPlaySessions(playSessionList1).Build(),
            };

            return taskItemList;
        }

        #endregion
    }
}
