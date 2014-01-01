using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMaster.Model;

namespace TaskMaster.Test
{
    [TestFixture]
    public class PlaySessionTests
    {
        [Test]
        public void Given_two_play_sessions_with_the_same_date_and_different_played_times_then_they_are_equal_and_have_the_same_hashcode()
        {
            // Given
            var date = new DateTime(2013, 1, 3);
            var playSession1 = new PlaySession(date, new TimeSpan(2, 3, 4));
            var playSession2= new PlaySession(date, new TimeSpan(3, 2, 5));

            // When
            var equalsResult = playSession1.Equals(playSession1, playSession2);
            var hashCode1 = playSession1.GetHashCode(playSession1);
            var hashCode2 = playSession1.GetHashCode(playSession2);

            // Then
            Assert.IsTrue(equalsResult);
            Assert.IsTrue(hashCode1 == hashCode2);
        }

        [Test]
        public void Given_two_play_sessions_with_different_dates_and_same_played_times_then_they_are_not_equal_and_have_different_hashcodes()
        {
            // Given
            var time = new TimeSpan(2, 3, 4);
            var playSession1 = new PlaySession(new DateTime(2013, 1, 3), time);
            var playSession2 = new PlaySession(new DateTime(2013, 1, 4), time);

            // When
            var equalsResult = playSession1.Equals(playSession1, playSession2);
            var hashCode1 = playSession1.GetHashCode(playSession1);
            var hashCode2 = playSession1.GetHashCode(playSession2);

            // Then
            Assert.IsFalse(equalsResult);
            Assert.IsFalse(hashCode1 == hashCode2);
        }

        [Test]
        public void Given_a_play_session_when_a_null_is_compared_with_a_play_session_then_they_are_not_equal()
        {
            // Given
            var playSession = new PlaySession(new DateTime(2013, 1, 3));

            // When
            var result1 = playSession.Equals(playSession, null);
            var result2 = playSession.Equals(null, playSession);

            // Then
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void Given_two_null_play_sessions_then_they_are_the_same_and_the_hashcode_is_minus_1()
        {
            // Given
            var playSession1 = new PlaySession(new DateTime(1, 2, 3), new TimeSpan(2, 3, 4));

            // When
            var equalsResult = playSession1.Equals(null, null);
            var hashCode = playSession1.GetHashCode(null);

            // Then
            Assert.IsTrue(equalsResult);
            Assert.IsTrue(hashCode == -1);
        }
    }
}
