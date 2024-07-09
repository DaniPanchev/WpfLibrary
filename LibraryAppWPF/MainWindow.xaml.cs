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

namespace LibraryAppWPF
{
    public partial class MainWindow : Window
    {
        private library library;

        public MainWindow()
        {
            InitializeComponent();
            library = new library();
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
                MessageBox.Show($"Found book: {book}", "Book Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Book not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnRemoveBookClicked(object sender, RoutedEventArgs e)
        {
            var title = PromptDialog("Remove Book", "Enter the book title to remove:");
            bool isRemoved = library.RemoveBook(title);

            if (isRemoved)
            {
                MessageBox.Show("Book removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
}