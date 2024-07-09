using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace LibraryAppWPF
{
    public partial class MainWindow : Window
    {
        private Library library;
        private const string FilePath = "save.txt";

        public MainWindow()
        {
            InitializeComponent();
            library = new Library();
            Application.Current.Exit += OnApplicationExit;
        }
        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            SaveBooksToFile();
        }
        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            SaveBooksToFile();
            MessageBox.Show("Books saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            LoadBooksFromFile();
            MessageBox.Show("Books loaded successfully.", "Load", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SaveBooksToFile()
        {
            List<Book> allBooks = library.GetAllBooks();
            string jsonString = JsonConvert.SerializeObject(allBooks, Formatting.Indented);
            File.WriteAllText(FilePath, jsonString);
        }
        private void LoadBooksFromFile()
        {
            if (File.Exists(FilePath))
            {
                string jsonString = File.ReadAllText(FilePath);
                List<Book> allBooks = JsonConvert.DeserializeObject<List<Book>>(jsonString);
                library.LoadBooks(allBooks);
                BooksListBox.ItemsSource = library.GetAllBooks();
            }
            else
            {
                MessageBox.Show("No saved books found.", "Load", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void OnAddBookClicked(object sender, RoutedEventArgs e)
        {
            var title = PromptDialog("Add Book", "Enter the book title:");
            var authorName = PromptDialog("Add Book", "Enter the author's full name:");
            var authorEmail = PromptDialog("Add Book", "Enter the author's email:");
            var authorGenderStr = PromptDialog("Add Book", "Enter the author's gender (M/F):");
            var priceStr = PromptDialog("Add Book", "Enter the book price:");

            if (double.TryParse(priceStr, out double price) && char.TryParse(authorGenderStr, out char authorGender))
            {
                Author author = new Author(authorName, authorEmail, authorGender);
                Book book = new Book(title, author, price);
                library.AddBook(book);
                MessageBox.Show("Book added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                BooksListBox.ItemsSource = library.GetAllBooks(); // Update the ListBox
            }
            else
            {
                MessageBox.Show("Invalid input. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSearchBookClicked(object sender, RoutedEventArgs e)
        {
            var title = PromptDialog("Search Book", "Enter the book title to search:");
            Book book = library.SearchBookByTitle(title);

            if (book != null)
            {
                MessageTextBlock.Text = $"Found book: {book}";
            }
            else
            {
                MessageTextBlock.Text = "Book not found.";
            }
        }

        private void OnRemoveBookClicked(object sender, RoutedEventArgs e)
        {
            var title = PromptDialog("Remove Book", "Enter the book title to remove:");
            bool isRemoved = library.RemoveBook(title);

            if (isRemoved)
            {
                MessageBox.Show("Book removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                BooksListBox.ItemsSource = library.GetAllBooks(); // Update the ListBox
            }
            else
            {
                MessageBox.Show("Book not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnListAllBooksClicked(object sender, RoutedEventArgs e)
        {
            BooksListBox.ItemsSource = library.GetAllBooks();
        }

        private void OnCheckWordClicked(object sender, RoutedEventArgs e)
        {
            var word = PromptDialog("Check Word", "Enter the word to check in book titles:");
            bool containsWord = library.ContainsTitle(word);

            if (containsWord)
            {
                MessageBox.Show($"Library contains a book with the word '{word}' in the title.", "Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"No books found with the word '{word}' in the title.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            SaveBooksToFile();
            Application.Current.Shutdown();
        }

        private string PromptDialog(string title, string message)
        {
            var dialog = new InputDialog(title, message);
            if (dialog.ShowDialog() == true)
            {
                return dialog.ResponseText;
            }
            return string.Empty;
        }
    }

    public class Library
    {
        private List<Book> books;

        public Library()
        {
            books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public Book SearchBookByTitle(string title)
        {
            return books.Find(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool RemoveBook(string title)
        {
            var book = SearchBookByTitle(title);
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
            return books.Exists(book => book.Title.Contains(word, StringComparison.OrdinalIgnoreCase));
        }

        public void LoadBooks(List<Book> loadedBooks)
        {
            books = loadedBooks ?? new List<Book>();
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
            return $"Author[name={FullName}, email={Email}, gender={Gender}]";
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
            return $"Book[title={Title}, {Author}, price={Price}]";
        }
    }
}