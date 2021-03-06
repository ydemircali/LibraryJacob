using LibraryJacob.Models;
using Nest;
using System;
using System.Collections.Generic;
 
namespace LibraryJacob.ElasticSearch
{
    public abstract class ElasticSearchService<T> : IElasticSearchService<T> where T : BaseEntity
    {
        private readonly ElasticClient _client;
        public ElasticSearchService(ElasticClientProvider provider)
        {
            _client = provider.ElasticClient;
        }

        public void CheckIndexExist(T logModel, string indexName)
        {
            var response = _client.Indices.Exists(indexName);
            if (!response.Exists)
            {
                _client.Indices.Create(indexName,
                     index => index.Map<T>(
                          x => x
                         .AutoMap()
                  ));
            }
            
        }

        public IEnumerable<T> All(string indexName)
        {
            return _client.Search<T>(search =>
                search.From(0).Size(1000).MatchAll().Index(indexName)).Documents;
        }
       

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public T Get(Guid id, string indexName)
        {
            var getResponse = _client.Get<T>(id, g => g.Index(indexName));
    
            return getResponse.Source;
        }

        public IndexResponse Save(T entity, string indexName)
        {
            CheckIndexExist(entity, indexName);
 
            var result = _client.Index<T>(entity, idx => idx.Index(indexName));
            return result;
        }

        public IEnumerable<T> Search()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}