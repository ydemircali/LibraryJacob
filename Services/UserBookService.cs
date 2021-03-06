using System;
using System.Collections.Generic;
using LibraryJacob.ElasticSearch;
using LibraryJacob.Models;
using Nest;

namespace LibraryJacob.Services
{
    public class UserBookService : ElasticSearchService<UserBookModel>
    {
        private readonly ElasticClient _client;
        public UserBookService(ElasticClientProvider provider) : base(provider)
        {
            _client = provider.ElasticClient;
        }

        public IReadOnlyCollection<UserBookModel> SearchUserBooks(string indexName, string userName)
        {
            var response = _client.Search<UserBookModel>(s => s
                            .Index(indexName)
                            .From(0)
                            .Size(1000)
                            .Sort(st => st.Descending(p => p.ActionDate))
                            .Query(q => q.Match(m => m.Field(f => f.UserName).Query(userName)))
            
            );
            
            return response.Documents;
        }
    }

}