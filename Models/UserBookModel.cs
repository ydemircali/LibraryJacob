using System;

namespace LibraryJacob.Models
{
    public class UserBookModel : BaseEntity
    {
        public string UserName { get; set; }
        public string BookTitle { get; set; }
        public string BookCategory { get; set; }
        public string BookSubCategory { get; set; }
        public string BookAuthor { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
    }
    
}