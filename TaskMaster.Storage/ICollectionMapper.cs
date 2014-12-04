using System;
using System.Collections.Generic;
namespace TaskMaster.Storage
{
    public interface ICollectionMapper<TInput, TOutput>
    {
        ICollection<TOutput> Map();
        CollectionMapper<TInput, TOutput> WithCollectionToMap(ICollection<TInput> collectionToMap);
        CollectionMapper<TInput, TOutput> WithPerItemMapper(Func<TInput, TOutput> mapperFunc);
    }
}
