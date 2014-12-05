using System;
using System.Collections.Generic;
namespace TaskMaster.Storage
{
    public interface ICollectionMapper<TInput, TOutput>
    {
        ICollection<TOutput> Map();
        ICollectionMapper<TInput, TOutput> WithCollectionToMap(ICollection<TInput> collectionToMap);
        ICollectionMapper<TInput, TOutput> WithPerItemMapper(Func<TInput, TOutput> mapperFunc);
    }
}
