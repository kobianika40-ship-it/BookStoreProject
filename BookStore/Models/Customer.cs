namespace BookStore.Models
{
    internal class Customer : User
    {
        public List<Book> Library { get; set; } = new List<Book>();

        public Customer(string fullname, string email, string password, string phoneNumber)
            : base(fullname, email, password, phoneNumber)
        {
        }

        public decimal GetTotalPrice()
        {
            decimal total = 0;

            foreach (Book book in Library)
            {
                total += book.Price;
            }

            return total;
        }
    }
}
