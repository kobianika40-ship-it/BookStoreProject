using BookStore.Enums;
using BookStore.Helpers;
using BookStore.Models;

namespace BookStore
{
    internal static class BookStoreApp
    {
        private static List<Book> _books = new List<Book>();

        private static List<Customer> _users = new List<Customer>();

        public static async Task RunAsync()
        {
            CreateCatalog();

            _users = await JsonHelper<Customer>.ReadFromJsonAsync(FILE_PATHS.UsersPath);

            User.UserCount = 0;
            foreach (Customer user in _users)
            {
                if (user.Id > User.UserCount)
                {
                    User.UserCount = user.Id;
                }
            }

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("===== Book Store =====");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Log in");
                Console.WriteLine("0. Exit");

                int choice = ReadInt("Choose an option", 0, 2);

                try
                {
                    switch (choice)
                    {
                        case 1:
                            await RegisterAsync();
                            break;
                        case 2:
                            await LoginAsync();
                            break;
                        case 0:
                            running = false;
                            break;
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"File error: {ex.Message}");
                }
            }

            Console.WriteLine("Goodbye!");
        }

        private static void CreateCatalog()
        {
            string[] authorNames =
            {
                "Shota Rustaveli",
                "Konstantine Gamsakhurdia",
                "Mikheil Javakhishvili",
                "Nodar Dumbadze",
                "Ilia Chavchavadze",
                "George Orwell",
                "Antoine de Saint-Exupéry",
                "Fyodor Dostoevsky",
                "Gabriel García Márquez"
            };

            foreach (string name in authorNames)
            {
                Author.Add(name);
            }

            string[] titles =
            {
                "The Knight in the Panther's Skin",
                "The Right Hand of the Grand Master",
                "Jaqo's Dispossessed",
                "Me, Grandma, Iliko and Illarion",
                "Is That a Man?!",
                "1984",
                "Animal Farm",
                "The Little Prince",
                "Crime and Punishment",
                "One Hundred Years of Solitude"
            };
            int[] authorIndexes = { 0, 1, 2, 3, 4, 5, 5, 6, 7, 8 };
            int[] years = { 1200, 1939, 1925, 1960, 1863, 1949, 1945, 1943, 1866, 1967 };
            decimal[] prices = { 25.00m, 22.50m, 18.00m, 15.00m, 12.00m, 24.00m, 14.00m, 16.50m, 28.00m, 30.00m };

            for (int i = 0; i < titles.Length; i++)
            {
                Author author = Author.AllAuthors[authorIndexes[i]];
                _books.Add(new Book(titles[i], author, years[i], prices[i]));
            }
        }

        private static async Task RegisterAsync()
        {
            Console.WriteLine();
            Console.WriteLine("--- Registration ---");

            string fullname = ReadText("Full name");

            string email;
            while (true)
            {
                email = ReadText("Email");

                if (!email.Contains('@') || !email.Contains('.'))
                {
                    Console.WriteLine("Invalid email! e.g.: giorgi@gmail.com");
                }
                else if (FindUserByEmail(email) != null)
                {
                    Console.WriteLine("This email is already registered!");
                }
                else
                {
                    break;
                }
            }

            string password;
            while (true)
            {
                password = ReadText("Password (at least 6 characters)");

                if (password.Length >= 6)
                {
                    break;
                }

                Console.WriteLine("Password is too short!");
            }

            string phone;
            while (true)
            {
                phone = ReadText("Phone (9 digits)");

                if (phone.Length == 9 && phone.All(char.IsDigit))
                {
                    break;
                }

                Console.WriteLine("Phone must consist of exactly 9 digits!");
            }

            Customer customer = new Customer(fullname, email, password, phone);
            _users.Add(customer);
            await SaveUsersAsync();

            Console.WriteLine("Registration completed successfully!");
        }

        private static async Task LoginAsync()
        {
            Console.WriteLine();
            Console.WriteLine("--- Login ---");

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                string email = ReadText("Email");
                string password = ReadText("Password");

                Customer? user = FindUserByEmail(email);

                if (user != null && user.Password == password)
                {
                    Console.WriteLine($"Welcome, {user.Fullname}!");
                    await CustomerMenuAsync(user);
                    return;
                }

                Console.WriteLine($"Incorrect email or password! (attempt {attempt}/3)");
            }

            Console.WriteLine("No attempts left.");
        }

        private static Customer? FindUserByEmail(string email)
        {
            foreach (Customer user in _users)
            {
                if (user.Email.ToLower() == email.ToLower())
                {
                    return user;
                }
            }

            return null;
        }

        private static async Task CustomerMenuAsync(Customer user)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine($"===== {user.Fullname} =====");
                Console.WriteLine("1. View all books");
                Console.WriteLine("2. Search books by title");
                Console.WriteLine("3. Search books by author");
                Console.WriteLine("4. List of authors");
                Console.WriteLine("5. Add a book to my library");
                Console.WriteLine("6. My library");
                Console.WriteLine("7. Remove a book from my library");
                Console.WriteLine("8. Save my library to a text file");
                Console.WriteLine("9. Delete the text file");
                Console.WriteLine("0. Log out");

                int choice = ReadInt("Choose an option", 0, 9);

                try
                {
                    switch (choice)
                    {
                        case 1:
                            PrintBooks(_books);
                            break;
                        case 2:
                            SearchByTitle();
                            break;
                        case 3:
                            SearchByAuthor();
                            break;
                        case 4:
                            PrintAuthors();
                            break;
                        case 5:
                            await AddToLibraryAsync(user);
                            break;
                        case 6:
                            PrintLibrary(user);
                            break;
                        case 7:
                            await RemoveFromLibraryAsync(user);
                            break;
                        case 8:
                            SaveLibraryToTxt(user);
                            break;
                        case 9:
                            DeleteTxt();
                            break;
                        case 0:
                            running = false;
                            break;
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"File error: {ex.Message}");
                }
            }
        }

        private static void SearchByTitle()
        {
            string text = ReadText("Enter a title or part of it");

            List<Book> found = new List<Book>();
            foreach (Book book in _books)
            {
                if (book.Title.ToLower().Contains(text.ToLower()))
                {
                    found.Add(book);
                }
            }

            PrintBooks(found);
        }

        private static void SearchByAuthor()
        {
            string text = ReadText("Enter the author's first or last name");

            List<Author> authors = Author.Search(text);
            List<Book> found = new List<Book>();

            foreach (Author author in authors)
            {
                foreach (Book book in _books)
                {
                    if (book.Author.Id == author.Id)
                    {
                        found.Add(book);
                    }
                }
            }

            PrintBooks(found);
        }

        private static async Task AddToLibraryAsync(Customer user)
        {
            PrintBooks(_books);

            int id = ReadInt("Enter the book Id (0 - cancel)", 0, int.MaxValue);
            if (id == 0)
            {
                return;
            }

            Book? book = FindBookById(_books, id);
            if (book == null)
            {
                Console.WriteLine("Book not found!");
                return;
            }

            if (FindBookById(user.Library, id) != null)
            {
                Console.WriteLine("This book is already in your library!");
                return;
            }

            user.Library.Add(book);
            await SaveUsersAsync();

            Console.WriteLine($"Book \"{book.Title}\" was added to your library.");
        }

        private static async Task RemoveFromLibraryAsync(Customer user)
        {
            if (user.Library.Count == 0)
            {
                Console.WriteLine("The library is empty.");
                return;
            }

            PrintLibrary(user);

            int id = ReadInt("Enter the Id of the book to remove (0 - cancel)", 0, int.MaxValue);
            if (id == 0)
            {
                return;
            }

            Book? book = FindBookById(user.Library, id);
            if (book == null)
            {
                Console.WriteLine("This book is not in your library!");
                return;
            }

            user.Library.Remove(book);
            await SaveUsersAsync();

            Console.WriteLine("The book was removed from your library.");
        }

        private static Book? FindBookById(List<Book> books, int id)
        {
            foreach (Book book in books)
            {
                if (book.Id == id)
                {
                    return book;
                }
            }

            return null;
        }

        private static void SaveLibraryToTxt(Customer user)
        {
            if (user.Library.Count == 0)
            {
                Console.WriteLine("The library is empty, nothing to save.");
                return;
            }

            List<string> lines = new List<string>();
            lines.Add($"Library: {user.Fullname}");

            foreach (Book book in user.Library)
            {
                lines.Add(book.ToString());
            }

            lines.Add($"Total: {user.GetTotalPrice():0.00} ₾");

            FileHelper<string>.WriteInFile(FILE_PATHS.LibraryTxtPath, lines);

            Console.WriteLine($"File created: {PathHelper.GetPath(FILE_PATHS.LibraryTxtPath)}");
            Console.WriteLine();
            FileHelper<string>.ReadFromFile(FILE_PATHS.LibraryTxtPath);
        }

        private static void DeleteTxt()
        {
            if (FileHelper<string>.DeleteFile(FILE_PATHS.LibraryTxtPath))
            {
                Console.WriteLine("File deleted.");
            }
            else
            {
                Console.WriteLine("The file does not exist yet.");
            }
        }

        private static void PrintBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"Id",-4}{"Title",-34}{"Author",-28}{"Year",-7}Price");

            foreach (Book book in books)
            {
                Console.WriteLine($"{book.Id,-4}{book.Title,-34}{book.Author.Name,-28}{book.Year,-7}{book.Price:0.00} ₾");
            }
        }

        private static void PrintAuthors()
        {
            Console.WriteLine();
            foreach (Author author in Author.AllAuthors)
            {
                Console.WriteLine($"{author.Id}. {author.Name}");
            }
        }

        private static void PrintLibrary(Customer user)
        {
            if (user.Library.Count == 0)
            {
                Console.WriteLine("Your library is empty.");
                return;
            }

            PrintBooks(user.Library);

            Console.WriteLine();
            Console.WriteLine($"Number of books: {user.Library.Count}");
            Console.WriteLine($"Total price: {user.GetTotalPrice():0.00} ₾");
        }

        private static string ReadText(string label)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                string text = (Console.ReadLine() ?? "").Trim();

                if (text.Length > 0)
                {
                    return text;
                }

                Console.WriteLine("This field cannot be empty!");
            }
        }

        private static int ReadInt(string label, int min, int max)
        {
            while (true)
            {
                Console.Write($"{label}: ");
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int number) && number >= min && number <= max)
                {
                    return number;
                }

                Console.WriteLine($"Enter a number from {min} to {max}!");
            }
        }

        private static async Task SaveUsersAsync()
        {
            await JsonHelper<Customer>.WriteInJsonAsync(FILE_PATHS.UsersPath, _users);
        }
    }
}
