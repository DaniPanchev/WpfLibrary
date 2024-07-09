using System.Globalization;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;

namespace Library
{
    internal class Program
    {
        static void AddBookToLibrary(library library)
        {
            Console.WriteLine("\nAdding a new book:");
            Console.Write("Enter the book title: ");
            string title = Console.ReadLine();
            Console.Write("Enter the author's full name: ");
            string authorName = Console.ReadLine();
            Console.Write("Enter the author's email: ");
            string authorEmail = Console.ReadLine();
            Console.Write("Enter the author's gender (M/F): ");
            char authorGender = char.Parse(Console.ReadLine());
            Console.Write("Enter the book price: ");
            double price = double.Parse(Console.ReadLine());
            Author author = new Author(authorName, authorEmail, authorGender);
            Book book = new Book(title, author, price);
            library.AddBook(book);
            Console.WriteLine("Book added successfully.");
        }
        static void SearchBookInLibrary(library library)
        {
            Console.WriteLine("\nSearching for book:");
            Console.Write("Enter the book for search: ");
            string title = Console.ReadLine();

            Book book = library.SearchBookByTitle(title);
            if (book != null)
            {
                Console.WriteLine($"Found book: {book}");
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }
        static void RemoveBookFromLibrary(library library)
        {
            Console.WriteLine("\nRemoving a book by title:");
            Console.Write("Enter the book title to remove: ");
            string title = Console.ReadLine();

            bool isRemoved = library.RemoveBook(title);
            if (isRemoved)
            {
                Console.WriteLine("Book removed successfully.");
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }
        static void ListAllBooks(library library)
        {
            Console.WriteLine("\nListing all books in the library:");
            List<Book> allBooks = library.GetAllBooks();
            if (allBooks.Count > 0)
            {
                foreach (Book book in allBooks)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("No books in the library.");
            }
        }
        static void CheckBookTitleContainsWord(library library)
        {
            Console.WriteLine("\nChecking if any book title contains a specific word:");
            Console.Write("Enter the word to check in book titles: ");
            string word = Console.ReadLine();

            bool containsWord = library.ContainsTitle(word);
            if (containsWord)
            {
                Console.WriteLine($"Library contains a book with the word '{word}' in the title.");
            }
            else
            {
                Console.WriteLine($"No books found with the word '{word}' in the title.");
            }
        }
    }
    public class Author
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }

        public Author(string fullName, string email, char gender)
        {
            FullName = fullName;
            Email = email;
            Gender = gender;
        }
        public override string ToString()
        {
            return $"Author[ name:{FullName} ,\nemail:{Email},\ngender:{Gender}]";
        }
    }
    public class Book
    {
        public string Title { get; set; }
        public Author Author { get; set; }
        public double Price { get; set; }

        public Book(string title, Author author, double price)
        {
            Title = title;
            Author = author;
            Price = price;
        }
        public override string ToString()
        {
            return $"Book[ name:{Title} , Author[{Author}],price={Price} lv.]";
        }
    }
    public class library
    {
        private List<Book> books;
        public library()
        {
            books = new List<Book>();
        }
        public void AddBook(Book book)
        {
            books.Add(book);
        }
        public Book SearchBookByTitle(string title)
        {
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Title.ToLower() == title.ToLower())
                {
                    return books[i];
                }
            }
            return null;
        }
        public bool RemoveBook(string title)
        {
            Book book = SearchBookByTitle(title);
            if (book != null)
            {
                books.Remove(book);
                return true;
            }
            return false;
        }
        public List<Book> GetAllBooks()
        {
            return new List<Book>(books);
        }
        public bool ContainsTitle(string word)
        {
            foreach (Book book in books)
            {
                if (book.Title.ToLower().Contains(word.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
