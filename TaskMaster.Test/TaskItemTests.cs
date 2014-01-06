using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMaster.Model;
using NUnit.Framework;

namespace TaskMaster.Test
{
    [TestFixture]
    public class TaskItemTests
    {
        [Test]
        public void Given_two_tasks_with_the_same_guid_when_they_are_compared_with_equals_then_true_is_returned()
        {
            // Given
            var task1 = new TaskItemBuilder(new Guid("94C33ADA-04F8-4305-8BBE-EF50EAF7E8EA")).WithDescription("bla bla").Build();
            var task2 = new TaskItemBuilder(new Guid("94C33ADA-04F8-4305-8BBE-EF50EAF7E8EA")).WithDescription("intentionally different description").Build();

            // When
            var result = task1.Equals(task2);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_a_task_with_an_id_when_its_hash_code_is_obtained_then_their_hash_codes_are_the_same()
        {
            // Given
            var id = new Guid("94C33ADA-04F8-4305-8BBE-EF50EAF7E8EA");
            var task = new TaskItemBuilder(id).WithDescription("bla bla").Build();

            // When
            var hashCode = task.GetHashCode();

            // Then 
            Assert.AreEqual(hashCode, id.GetHashCode());
        }

        [Test]
        public void When_a_task_is_constructed_its_playing_state_is_null_by_default()
        {
            // Given && when
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");

            // Then
            Assert.IsTrue(taskItem.PlayingState == null);
        }

        [TestCase("1/1/2013 00:00:00", "1/1/2013 23:59:59", true)]
        [TestCase("1/1/2013 01:02:00", "2/1/2013 05:02:00", false)]
        public void Given_two_date_times_then_they_are_in_the_same_day_when_appropiate(string date1str, string date2str, bool expectedResult)
        {
            // Given
            var date1 = DateTime.Parse(date1str);
            var date2 = DateTime.Parse(date2str);
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");

            // When
            var result = taskItem.DateTimesInSameDay(date1, date2);

            // Then
            Assert.That(result == expectedResult);
        }

        [Test]
        public void Given_a_play_session_that_has_been_added_then_there_is_a_played_session_for_that_day()
        {
            // Given
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            var playSessionDate = new DateTime(2013, 1, 1);
            taskItem.PlayASession(playSessionDate);

            // When
            var result = taskItem.ThereIsAPlayedSessionThatDay(playSessionDate);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_a_task_that_was_played_then_there_is_not_a_played_session_for_another_day()
        {
            // Given
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            var playSessionDate = new DateTime(2013, 1, 1);
            taskItem.PlayASession(playSessionDate);

            // When
            var result = taskItem.ThereIsAPlayedSessionThatDay(new DateTime(2013, 1, 2));

            // Then
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_a_task_when_correct_play_sessions_are_loaded_then_its_play_sessions_are_those()
        {
            // Given
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            var playSessions = new List<PlaySession>()
            {
                new PlaySession(new DateTime(1999, 1, 1), new TimeSpan(0, 15, 0)),
                new PlaySession(new DateTime(1999, 1, 2), new TimeSpan(1, 20, 0)),
            };

            // When
            taskItem.LoadPlaySessions(playSessions);

            // Then
            Assert.IsTrue(taskItem.PlaySessions.Count() == playSessions.Count);
            Assert.IsTrue(taskItem.PlaySessions.ToList()[0] == playSessions[0]);
            Assert.IsTrue(taskItem.PlaySessions.ToList()[1] == playSessions[1]);
        }

        [Test]
        public void Given_a_task_when_a_play_session_list_with_repeated_date_play_sessions_is_loaded_then_an_argument_exception_is_thrown()
        {
            // Given
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            var playSessions = new List<PlaySession>()
            {
                new PlaySession(new DateTime(1999, 1, 1), new TimeSpan(0, 15, 0)),
                new PlaySession(new DateTime(1999, 1, 1), new TimeSpan(1, 20, 0)),
            };

            // When
            Assert.That(() => taskItem.LoadPlaySessions(playSessions), 
                Throws.Exception
                .TypeOf<ArgumentException>());
        }

        [Test]
        public void Given_a_task_with_play_sessions_the_total_played_time_is_correctly_calculated()
        {
            // Given
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            taskItem.LoadPlaySessions(new List<PlaySession>() 
                { 
                    new PlaySession(new DateTime(3, 4, 3), new TimeSpan(2, 3, 4)),
                    new PlaySession(new DateTime(3, 4, 4), new TimeSpan(1, 0, 5)),
                }
            );

            // When
            var totalTime = taskItem.TotalPlayedTime;

            // Then
            Assert.IsTrue(totalTime == new TimeSpan(3, 3, 9));
        }

        [Test]
        public void Given_a_task_with_null_play_sessions_the_total_played_time_is_zero()
        {
            // Given
            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            taskItem.LoadPlaySessions(null);

            // When
            var totalTime = taskItem.TotalPlayedTime;

            // Then
            Assert.IsTrue(totalTime == new TimeSpan(0));
        }

        [Test]
        public void Given_a_task_with_play_sessions_the_today_played_time_is_correctly_calculated()
        {
            // Given
            var todayDate = new DateTime(2013, 2, 3);

            var taskItem = new TaskItem(Guid.NewGuid(), "bla bla");
            taskItem.LoadPlaySessions(new List<PlaySession>() 
                { 
                    new PlaySession(new DateTime(3, 4, 3), new TimeSpan(2, 3, 4)),
                    new PlaySession(todayDate, new TimeSpan(1, 0, 5)),
                }
            );

            // When
            var playedTime = taskItem.GetPlayedTimeForDay(todayDate);

            // Then
            Assert.IsTrue(playedTime == new TimeSpan(1, 0, 5));
        }
    }
}
