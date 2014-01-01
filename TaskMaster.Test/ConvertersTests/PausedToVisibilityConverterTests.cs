using NUnit.Framework;
using System.Windows;
using TaskMaster.Model;
using TaskMaster.Model.Converters;

namespace TaskMaster.Test
{
    [TestFixture]
    public class PausedToVisibilityConverterTests
    {
        [Test]
        public void When_Paused_is_passed_as_value_then_Visibility_Visible_is_returned()
        {
            // Given
            var converter = new PausedToVisibilityConverter();
            var state = PlayingState.Paused;

            // When
            var result = (Visibility)converter.Convert(state, typeof(PlayingState), null, null);

            // Then
            Assert.IsTrue(result == Visibility.Visible);
        }

        [Test]
        public void When_null_is_passed_as_value_then_Visibility_Collapsed_is_returned()
        {
            // Given
            var converter = new PausedToVisibilityConverter();

            // When
            var result = (Visibility)converter.Convert(null, typeof(PlayingState), null, null);

            // Then
            Assert.IsTrue(result == Visibility.Collapsed);
        }

        [Test]
        public void When_other_PlayingState_is_passed_as_value_then_Visibility_Collapsed_is_returned()
        {
            // Given
            var converter = new PausedToVisibilityConverter();
            var state = PlayingState.Playing;

            // When
            var result = (Visibility)converter.Convert(state, typeof(PlayingState), null, null);

            // Then
            Assert.IsTrue(result == Visibility.Collapsed);
        }
    }
}
