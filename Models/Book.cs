namespace LibraryJacob.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Page { get; set; }
        public string Genre { get; set; }
        public string SubGenre { get; set; }
        public string Publisher { get; set; }
    }
}