namespace BookStore.Models
{
    internal class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static int AuthorCount { get; set; }

        public static List<Author> AllAuthors { get; } = new List<Author>();

        public Author(string name)
        {
            AuthorCount++;
            Id = AuthorCount;
            Name = name;
        }

        public static Author Add(string name)
        {
            Author author = new Author(name);
            AllAuthors.Add(author);
            return author;
        }

        public static List<Author> Search(string text)
        {
            List<Author> result = new List<Author>();

            foreach (Author author in AllAuthors)
            {
                if (author.Name.ToLower().Contains(text.ToLower()))
                {
                    result.Add(author);
                }
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Id}|{Name}";
        }
    }
}
