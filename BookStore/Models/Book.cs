namespace BookStore.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public static int BookCount { get; set; }

        public Book(string title, Author author, int year, decimal price)
        {
            BookCount++;
            Id = BookCount;
            Title = title;
            Author = author;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Id}|{Title}|{Author.Name}|{Year}|{Price}";
        }
    }
}
