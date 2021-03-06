using System;
using System.Collections.Generic;
using Nest;

namespace LibraryJacob.ElasticSearch
{
    public interface IElasticSearchService<T> where T : class
    {
        void CheckIndexExist(T logMode, string indexName);

        IEnumerable<T> All(string indexName);

        IndexResponse Save(T entity, string indexName);
        T Get(Guid id, string indexName);
        void Update(T entity);
        bool Delete(Guid id);
        IEnumerable<T> Search();
    }
}