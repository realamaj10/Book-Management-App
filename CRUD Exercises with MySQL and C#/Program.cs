using System;

namespace BookManagementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BookService bookService = new BookService(); // Creating an instance of BookService
            bool exit = false; // Flag to control the loop

            while (!exit)
            {
                // Displaying the main menu
                Console.WriteLine("\nBook Management Application");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. View all books");
                Console.WriteLine("3. View books by author");
                Console.WriteLine("4. Update a book's details");
                Console.WriteLine("5. Delete a book");
                Console.WriteLine("6. Search books by genre");
                Console.WriteLine("7. Sort and paginate books");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                // Reading user's choice
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        bookService.InsertNewBook();
                        break;
                    case "2":
                        bookService.RetrieveAllBooks();
                        break;
                    case "3":
                        bookService.RetrieveBooksByAuthor();
                        break;
                    case "4":
                        bookService.UpdateBook();
                        break;
                    case "5":
                        bookService.DeleteBook();
                        break;
                    case "6":
                        // Search books by genre
                        Console.Write("Enter genre to search: ");
                        string genre = Console.ReadLine();
                        bookService.RetrieveBooks(genreFilter: genre);
                        break;
                    case "7":
                        // Sort and paginate books
                        Console.Write("Enter genre to filter (optional): ");
                        genre = Console.ReadLine();
                        Console.Write("Enter sorting criteria (Title, Author, PublishedYear): ");
                        string sortBy = Console.ReadLine();
                        Console.Write("Enter page number: ");
                        int page = int.Parse(Console.ReadLine());
                        bookService.RetrieveBooks(genreFilter: genre, sortBy: sortBy, page: page);
                        break;
                    case "8":
                        exit = true; // Exiting the application
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}
