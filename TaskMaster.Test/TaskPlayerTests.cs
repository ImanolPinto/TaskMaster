using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaskMaster.Model;
using Moq;
using System.Collections.ObjectModel;

namespace TaskMaster.Test
{
    [TestFixture]
    public class TaskPlayerTests
    {
        [Test]
        public void Given_a_task_that_is_null_then_CanPlay_returns_false()
        {
            var sut = TaskPlayerThatPlaysNow();
            var result = sut.CanPlay(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void Given_a_task_that_is_null_then_CanPause_returns_false()
        {
            var sut = TaskPlayerThatPlaysNow();
            var result = sut.CanPause(null);

            Assert.IsFalse(result);
        }

        [Test]
        public void Only_a_task_that_it_is_not_playing_can_be_played()
        {
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var sut = TaskPlayerThatPlaysNow();

            task.PlayingState = PlayingState.Playing;
            var canPlayPlaying = sut.CanPlay(task);

            task.PlayingState = PlayingState.Completed;
            var canPlayCompleted = sut.CanPlay(task);

            task.PlayingState = PlayingState.None;
            var canPlayNone = sut.CanPlay(task);

            task.PlayingState = PlayingState.Paused;
            var canPlayPaused = sut.CanPlay(task);

            task.PlayingState = PlayingState.Stopped;
            var canPlayStopped = sut.CanPlay(task);

            Assert.IsFalse(canPlayPlaying);
            Assert.IsTrue(canPlayCompleted);
            Assert.IsTrue(canPlayNone);
            Assert.IsTrue(canPlayPaused);
            Assert.IsTrue(canPlayStopped);
        }

        [Test]
        public void Only_a_task_that_it_is_playing_can_be_paused()
        {
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var sut = TaskPlayerThatPlaysNow();

            task.PlayingState = PlayingState.Playing;
            var canPausePlayingTask = sut.CanPause(task);

            task.PlayingState = PlayingState.Completed;
            var canPauseCompletedTask = sut.CanPause(task);

            task.PlayingState = PlayingState.None;
            var canPauseNoneTask = sut.CanPause(task);

            task.PlayingState = PlayingState.Paused;
            var canPausePausedTask = sut.CanPause(task);

            task.PlayingState = PlayingState.Stopped;
            var canPauseStoppedTask = sut.CanPause(task);

            Assert.IsTrue(canPausePlayingTask);
            Assert.IsFalse(canPauseCompletedTask);
            Assert.IsFalse(canPauseNoneTask);
            Assert.IsFalse(canPausePausedTask);
            Assert.IsFalse(canPauseStoppedTask);
        }

        [Test]
        public void Given_a_task_that_can_be_played_when_it_is_played_the_ellapsed_time_is_not_null_and_its_state_is_changed_to_playing()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var sut = TaskPlayerThatPlaysNow();
            Assert.IsTrue(sut.CanPlay(task));

            // When
            sut.Play(task);

            // Then
            Assert.IsTrue(sut.EllapsedPlayingTime != null);
            Assert.IsTrue(task.PlayingState == PlayingState.Playing);
        }

        [Test]
        public void Given_a_task_that_can_be_played_and_has_not_been_ever_played_when_it_is_played_then_the_task_played_session_list_contains_a_single_item_with_that_date()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var playTime = new DateTime(2013, 1, 1);
            var sut = TaskPlayerThatPlaysAt(playTime);

            // When
            sut.Play(task);

            // Then
            Assert.IsTrue(task.PlaySessions.Count == 1);
            Assert.IsTrue(task.PlaySessions[0].Date == playTime);
        }

        [Test]
        public void Given_a_task_that_can_be_played_and_was_played_once_another_day_when_it_is_played_then_the_task_played_session_list_contains_a_new_item_with_the_date()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var previousPlayedDate = new DateTime(2013, 1, 1);
            task.PlayASession(previousPlayedDate);

            var playTime = new DateTime(2013, 1, 2);
            var sut = TaskPlayerThatPlaysAt(playTime);

            // When
            sut.Play(task);

            // Then
            Assert.IsTrue(task.PlaySessions.Count == 2);
            Assert.IsTrue(task.PlaySessions[1].Date == playTime);
        }

        [Test]
        public void Given_a_task_that_can_be_played_and_was_played_the_same_day_when_it_is_played_then_the_task_played_session_list_contains_the_same_item()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var playTime = new DateTime(2013, 1, 1);
            task.PlayASession(playTime);
            var sut = TaskPlayerThatPlaysAt(playTime);

            // When
            sut.Play(task);

            // Then
            Assert.IsTrue(task.PlaySessions.Count == 1);
            Assert.IsTrue(task.PlaySessions[0].Date == playTime);
        }

        [Test]
        public void Given_a_task_that_can_be_played_and_has_not_been_played_that_day_when_it_is_played_its_play_session_timer_increases_the_ellapsed_time()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var sut = TaskPlayerThatPlaysNow();
            sut.InstantiateTimerWithInterval(1);

            // When
            sut.Play(task);
            Thread.Sleep(30);  // wait for an increase of ellapsed time

            // Then
            Assert.IsTrue(task.PlaySessions[0].PlayedTime.Ticks > 0);
        }

        [Test]
        public void Given_a_task_that_can_be_played_and_has_been_played_that_day_when_it_is_played_its_play_session_timer_adds_the_ellapsed_time()
        {
            // Given
            var date = new DateTime(2012, 1, 1);
            var playedTime = new TimeSpan(1, 30, 0);
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            task.PlayASession(date);
            task.PlaySessions[0].PlayedTime = playedTime;

            var sut = TaskPlayerThatPlaysAt(date);
            sut.InstantiateTimerWithInterval(1);

            // When
            sut.Play(task);
            Thread.Sleep(30);  // wait for an increase of ellapsed time

            // Then
            Assert.IsTrue(task.PlaySessions[0].PlayedTime > playedTime);
        }

        [Test]
        public void Given_a_task_that_can_be_played_and_was_played_yesterday_and_has_been_played_that_day_when_it_is_played_its_play_session_timer_adds_the_ellapsed_time_to_the_correct_PlaySession()
        {
            // Given
            var yesterday = new DateTime(2012, 1, 1);
            var today = yesterday.AddDays(1);
            var yesterdayPlayedTime = new TimeSpan(0, 15, 0);
            var todayInitialPlayedTime = new TimeSpan(1, 30, 0);
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();

            task.PlayASession(today);
            task.PlaySessions[0].PlayedTime = todayInitialPlayedTime;
            task.PlayASession(yesterday);
            task.PlaySessions[1].PlayedTime = yesterdayPlayedTime;

            var sut = TaskPlayerThatPlaysAt(today);
            sut.InstantiateTimerWithInterval(1);

            // When
            sut.Play(task);
            Thread.Sleep(30);  // wait for an increase of ellapsed time

            // Then
            Assert.IsTrue(task.PlaySessions[0].PlayedTime > todayInitialPlayedTime);
            Assert.IsTrue(yesterdayPlayedTime == task.PlaySessions[1].PlayedTime);
        }

        [Test]
        public void Given_a_task_that_it_is_playing_when_it_is_paused_the_ellapsed_time_is_does_not_increase_and_its_state_is_changed_to_paused()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var sut = TaskPlayerThatPlaysNow();
            sut.InstantiateTimerWithInterval(1);
            sut.Play(task);
            var ellapsedTimeBeforePause = sut.EllapsedPlayingTime;

            // When
            sut.Pause(task);
            Thread.Sleep(3);  // wait for an increase of ellapsed time

            // Then
            Assert.IsTrue(sut.EllapsedPlayingTime == ellapsedTimeBeforePause);
            Assert.IsTrue(task.PlayingState == PlayingState.Paused);
        }

        [Test]
        public void Given_an_ellapsed_time_when_the_task_player_is_Reset_then_its_ellapsed_time_is_zero()
        {
            // Given
            var task = new TaskItemBuilder(Guid.NewGuid()).Build();
            var sut = TaskPlayerThatPlaysNow();
            sut.InstantiateTimerWithInterval(1);
            sut.Play(task);
            Thread.Sleep(3);  // wait for an increase of ellapsed time

            // When
            sut.Reset();

            // Then
            Assert.IsTrue(sut.EllapsedPlayingTime == new TimeSpan());
        }

        private static TaskPlayer TaskPlayerThatPlaysNow()
        {
            return TaskPlayerThatPlaysAt(DateTime.Now);
        }

        private static TaskPlayer TaskPlayerThatPlaysAt(DateTime dateTime)
        {
            var timeProviderMock = new Mock<ITimeProvider>();
            timeProviderMock.Setup(x => x.Now()).Returns(dateTime);
            var sut = new TaskPlayer(timeProviderMock.Object);
            return sut;
        }
    }
}
