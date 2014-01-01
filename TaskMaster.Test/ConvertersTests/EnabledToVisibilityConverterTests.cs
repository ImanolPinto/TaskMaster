using NUnit.Framework;
using System.Windows;
using TaskMaster.Model.Converters;

namespace TaskMaster.Test
{
    [TestFixture]
    public class EnabledToVisibilityConverterTests
    {
        [Test]
        public void When_true_is_passed_as_value_then_the_converter_returns_Visibility_Visible()
        {
            // Given
            var converter = new EnabledToVisibilityConverter();

            // When
            var result = (Visibility) converter.Convert(true, typeof(bool), null, null);

            // Then
            Assert.IsTrue(result == Visibility.Visible);
        }

        [Test]
        public void When_false_is_passed_as_value_then_the_converter_returns_Visibility_Collapsed()
        {
            // Given
            var converter = new EnabledToVisibilityConverter();

            // When
            var result = (Visibility)converter.Convert(false, typeof(bool), null, null);

            // Then
            Assert.IsTrue(result == Visibility.Collapsed);
        }

        [Test]
        public void When_null_is_passed_as_value_then_the_converter_returns_Visibility_Collapsed()
        {
            // Given
            var converter = new EnabledToVisibilityConverter();

            // When
            var result = (Visibility)converter.Convert(null, typeof(bool), null, null);

            // Then
            Assert.IsTrue(result == Visibility.Collapsed);
        }
    }
}
