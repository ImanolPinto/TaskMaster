using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Storage.Tests
{
    [TestFixture]
    public class CollectionMapperTests
    {
        [Test]
        public void a_Given_a_collection_and_a_mapping_function_when_map_is_requested_then_the_mapping_function_is_called_for_each_element_of_the_collection_and_that_result_is_returned()
        {
            // Given
            var collectionToMap = new Collection<string>()
            {
                "hola",
                "ke",
                "ase"
            };
            Func<string, string> itemMapperThatAddsAZero = x => x + "0";

            var sut = new CollectionMapper<string, string>()
                .WithCollectionToMap(collectionToMap)
                .WithPerItemMapper(itemMapperThatAddsAZero);

            // When
            var mappedResult = sut.Map();

            // Then
            Assert.IsTrue(mappedResult.Count == 3);
            Assert.IsTrue(mappedResult.All(x => x.EndsWith("0")));
        }

        [Test]
        public void b_Given_an_empty_collection_when_Map_is_requested_then_an_empty_collection_is_returned()
        {
            // Given
            var collectionToMap = new Collection<object>();
            var sut = new CollectionMapper<object, object>()
                .WithCollectionToMap(collectionToMap)
                .WithPerItemMapper(x => { return x; });

            // When
            var result = sut.Map();

            // Then
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void c_Given_a_StoredTaskItem_list_with_null_items_when_Map_is_requested_then_null_items_are_ignored()
        {
            // Given
            var collectionToMap = new Collection<object>()
            {
                null,
                "hola",
                null,
                "ke",
                null
            };

            var sut = new CollectionMapper<object, object>()
               .WithCollectionToMap(collectionToMap)
               .WithPerItemMapper(x => { return x; });

            // When
            var result = sut.Map();

            // Then
            Assert.IsTrue(result.Count == 2);
        }
    }
}
