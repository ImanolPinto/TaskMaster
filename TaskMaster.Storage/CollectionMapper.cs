using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TaskMaster.Model;

namespace TaskMaster.Storage
{
    public class CollectionMapper<TInput, TOutput> : ICollectionMapper<TInput,TOutput>
    {
        Func<TInput, TOutput> _perItemMapper;
        ICollection<TInput> _collectionToMap;

        public ICollectionMapper<TInput, TOutput> WithPerItemMapper(Func<TInput, TOutput> mapperFunc)
        {
            _perItemMapper = mapperFunc;
            return this;
        }

        public ICollectionMapper<TInput, TOutput> WithCollectionToMap(ICollection<TInput> collectionToMap)
        {
            _collectionToMap = collectionToMap;
            return this;
        }

        public ICollection<TOutput> Map()
        {
            Ensure.IsNotNull("CollectionToMap", _collectionToMap);
            Ensure.IsNotNull("PerItemMapper", _perItemMapper);

            var mappedList = new Collection<TOutput>();
            foreach (var itemToMap in _collectionToMap)
            {
                var mappedItem = _perItemMapper(itemToMap);
                if (mappedItem == null)
                    continue;

                mappedList.Add(mappedItem);
            }

            _collectionToMap = null;
            _perItemMapper = null;

            return mappedList;
        }
    }
}
