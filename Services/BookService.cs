using LibraryJacob.ElasticSearch;
using LibraryJacob.Models;

namespace LibraryJacob.Services
{
    public class BookService : ElasticSearchService<Book>
    {
        public BookService(ElasticClientProvider provider) : base(provider)
        {
        }
    }

}