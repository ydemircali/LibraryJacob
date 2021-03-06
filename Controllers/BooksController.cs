using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using LibraryJacob.ElasticSearch;
using LibraryJacob.Models;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using LibraryJacob.Helper;
using LibraryJacob.Services;
using System;
using System.Threading.Tasks;

namespace LibraryJacob.Controllers
{
    public class BooksController : Controller
    {
        private string _bookIndexName;
        private string _userBookIndexName;
        private readonly BookService _bookService;
        private readonly UserBookService _userBookService;
        
        public BooksController(BookService bookService, UserBookService userBookService, IOptions<ElasticConnectionSettings> elasticConnection)
        {
            _bookService = bookService;
            _userBookService = userBookService;
            _bookIndexName = elasticConnection.Value.ElasticBookIndex;
            _userBookIndexName = elasticConnection.Value.ElasticUserBookIndex;
        }

        public IActionResult Index()
        {
            var books = _bookService.All(_bookIndexName);
            return View(books);
        }

        [Authorize]
        public IActionResult Detail(Guid id)
        {
            var book = _bookService.Get(id, _bookIndexName);
            var userBooks = _userBookService.SearchUserBooks(_userBookIndexName,User.Identity.Name).ToList();
            ViewData["AddedLibrary"] = userBooks.Any(s => s.Action == "AddMyLibrary" && s.BookTitle == book.Title) ? "1" : "0";

            if (book != null)
            {
                var userBookModel = new UserBookModel(){
                    Id = Guid.NewGuid(),
                    UserName = User.Identity.Name,
                    BookTitle = book.Title,
                    BookCategory = book.Genre,
                    BookSubCategory = book.SubGenre,
                    BookAuthor = book.Author,
                    Action = "Detail",
                    ActionDate = DateTime.Now
                };

                var response = _userBookService.Save(userBookModel, _userBookIndexName);
                if (!response.IsValid)
                {
                    throw new Exception(response.OriginalException.Message);
                }

            }

            return View(book);
        }

        [HttpPost]
        [Route("addMyLibrary")]
        public JsonResult AddMyLibrary(Guid bookId)
        {
            AjaxResult result = new AjaxResult();

            var book = _bookService.Get(bookId, _bookIndexName);
            var userBooks = _userBookService.SearchUserBooks(_userBookIndexName,User.Identity.Name).ToList();

            if (book != null && !userBooks.Any(a => a.Action == "AddMyLibrary" && a.BookTitle == book.Title))
            {
                var userBookModel = new UserBookModel(){
                    Id = Guid.NewGuid(),
                    UserName = User.Identity.Name,
                    BookTitle = book.Title,
                    BookCategory = book.Genre,
                    BookSubCategory = book.SubGenre,
                    BookAuthor = book.Author,
                    Action = "AddMyLibrary",
                    ActionDate = DateTime.Now
                };

                var response = _userBookService.Save(userBookModel, _userBookIndexName);
                if (!response.IsValid)
                {
                    result.IsSuccess = false;
                    result.Message = response.OriginalException.Message;
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "Added Your Library";
                }
            }

            return Json(result);
        }

        [Authorize]
        public IActionResult FillDataFromJsonToElastic()
        {
            var books = new List<BookJsonModel>();

            using (StreamReader r = new StreamReader("books_new.json"))
            {
                string json = r.ReadToEnd();
                books = JsonConvert.DeserializeObject<List<BookJsonModel>>(json);
            }
            
            foreach (var item in books)
            {
                var bookModel = new Book(){
                    Id = Guid.NewGuid(),
                    Title = item.Title,
                    Author = BookHelper.TitleNameSurname(item.Author),
                    Genre = BookHelper.GenreUpperCase(item.Genre),
                    SubGenre = BookHelper.GenreUpperCase(item.SubGenre),
                    Publisher = item.Publisher,
                    Page = item.Height
                };   

                var response = _bookService.Save(bookModel, _bookIndexName);
                if (!response.IsValid)
                {
                    throw new Exception(response.OriginalException.Message);
                }   

            }

            var booksElastic = _bookService.All(_bookIndexName);
            return Json(booksElastic);
        }

        [Authorize]
        [Route("Admin")]
        public IActionResult Admin()
        {
            if (User.Identity.Name != "ydemircali@gmail.com")
            {
                return Redirect("/Books");  
            }
            var userBooks = _userBookService.All(_userBookIndexName);

            return View(userBooks);
        }
    }
}
