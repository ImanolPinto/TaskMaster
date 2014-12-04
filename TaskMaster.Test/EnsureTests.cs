using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Model;

namespace TaskMaster.Test
{
    [TestFixture]
    public class EnsureTests
    {
        [Test]
        public void Given_a_null_object_when_isNotNull_is_ensured_then_ArgumentNullException_is_raised()
        {
            // Given
            Action given = () => Ensure.IsNotNull("object", null);

            // When && Then
            Assert.Throws<ArgumentNullException>(() => given());
        }

        [Test]
        public void Given_a_not_null_object_when_isNotNull_is_ensured_then_ArgumentNullException_is_raised()
        {
            // Given
            Action given = () => Ensure.IsNotNull("object", "string");

            // When && Then
            Assert.DoesNotThrow(() => given());
        }
    }
}
