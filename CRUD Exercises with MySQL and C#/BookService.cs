using System;
using MySql.Data.MySqlClient;

namespace BookManagementApp
{
    public class BookService
    {
        // Connection string for the MySQL database.
         private readonly string connectionString = "Server=localhost;Database=BookDB;User ID=root;Password=root;";

        // Method to insert a new book into the database.
        public void InsertNewBook()
        {
            // Prompting the user to enter book details.
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();
            Console.Write("Enter book author: ");
            string author = Console.ReadLine();
            Console.Write("Enter published year: ");
            int publishedYear = int.Parse(Console.ReadLine());
            Console.Write("Enter book genre: ");
            string genre = Console.ReadLine();

            try
            {
                // Establishing connection to the database.
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // SQL query to insert a new book.
                    string query = "INSERT INTO Books (Title, Author, PublishedYear, Genre) VALUES (@Title, @Author, @PublishedYear, @Genre)";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Adding parameters to prevent SQL injection.
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Author", author);
                        cmd.Parameters.AddWithValue("@PublishedYear", publishedYear);
                        cmd.Parameters.AddWithValue("@Genre", genre);
                        // Executing the query.
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Book inserted successfully.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handling MySQL specific errors.
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handling other potential errors.
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to retrieve and display all books.
        public void RetrieveAllBooks()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // SQL query to select all books.
                    string query = "SELECT * FROM Books";
                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Books:");
                        // Reading and displaying each book record.
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["BookID"]}, {reader["Title"]}, {reader["Author"]}, {reader["PublishedYear"]}, {reader["Genre"]}");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to retrieve books by a specific author.
        public void RetrieveBooksByAuthor()
        {
            Console.Write("Enter author name: ");
            string author = Console.ReadLine();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // SQL query to select books by the specified author.
                    string query = "SELECT * FROM Books WHERE Author = @Author";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Author", author);
                        using (var reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine($"Books by {author}:");
                            // Reading and displaying each book record.
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["BookID"]}, {reader["Title"]}, {reader["Author"]}, {reader["PublishedYear"]}, {reader["Genre"]}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to update a book's details based on BookID.
        public void UpdateBook()
        {
            Console.Write("Enter BookID of the book to update: ");
            int bookID = int.Parse(Console.ReadLine());

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Checking if the book exists before updating.
                    string checkQuery = "SELECT COUNT(*) FROM Books WHERE BookID = @BookID";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@BookID", bookID);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count == 0)
                        {
                            Console.WriteLine("Book not found.");
                            return;
                        }
                    }

                    // Prompting the user to enter new book details.
                    Console.Write("Enter new book title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter new book author: ");
                    string author = Console.ReadLine();
                    Console.Write("Enter new published year: ");
                    string publishedYearInput = Console.ReadLine();
                    int? publishedYear = null;
                    if (!string.IsNullOrEmpty(publishedYearInput) && int.TryParse(publishedYearInput, out int parsedYear))
                    {
                        publishedYear = parsedYear;
                    }
                    Console.Write("Enter new book genre: ");
                    string genre = Console.ReadLine();

                    // SQL query to update the book details.
                    string query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre";
                    if (publishedYear.HasValue)
                    {
                        query += ", PublishedYear = @PublishedYear";
                    }
                    query += " WHERE BookID = @BookID";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Author", author);
                        cmd.Parameters.AddWithValue("@Genre", genre);
                        cmd.Parameters.AddWithValue("@BookID", bookID);
                        if (publishedYear.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@PublishedYear", publishedYear.Value);
                        }

                        // Executing the update query.
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Book updated successfully.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to delete a book based on BookID.
        public void DeleteBook()
        {
            Console.Write("Enter BookID of the book to delete: ");
            int bookID = int.Parse(Console.ReadLine());

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Checking if the book exists before deleting.
                    string checkQuery = "SELECT COUNT(*) FROM Books WHERE BookID = @BookID";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@BookID", bookID);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count == 0)
                        {
                            Console.WriteLine("Book not found.");
                            return;
                        }
                    }

                    // Confirming the deletion.
                    Console.Write("Are you sure you want to delete this book? (yes/no): ");
                    string confirmation = Console.ReadLine();
                    if (confirmation.ToLower() != "yes")
                    {
                        Console.WriteLine("Deletion cancelled.");
                        return;
                    }

                    // SQL query to delete the book.
                    string query = "DELETE FROM Books WHERE BookID = @BookID";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@BookID", bookID);
                        // Executing the delete query.
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Book deleted successfully.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to retrieve books based on filters and sorting options.
        public void RetrieveBooks(string genreFilter = null, string sortBy = null, int page = 1, int pageSize = 10)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // SQL query to select books with optional filters and sorting.
                    string query = "SELECT * FROM Books";
                    if (!string.IsNullOrEmpty(genreFilter))
                    {
                        query += " WHERE Genre = @Genre";
                    }
                    if (!string.IsNullOrEmpty(sortBy))
                    {
                        query += $" ORDER BY {sortBy}";
                    }
                    query += " LIMIT @Offset, @PageSize";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(genreFilter))
                        {
                            cmd.Parameters.AddWithValue("@Genre", genreFilter);
                        }
                        cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        using (var reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Books:");
                            // Reading and displaying each book record.
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["BookID"]}, {reader["Title"]}, {reader["Author"]}, {reader["PublishedYear"]}, {reader["Genre"]}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
