using LibraryData;
using LibraryData.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryData.Repositories
{
    public class BookRepository
    {
        public bool AddBook(Book book)
        {
            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            string query = @"INSERT INTO Books
                             (BookId, Title, Author, Stock)
                             VALUES
                             (@BookId, @Title, @Author, @Stock)";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@BookId", book.BookId);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@Stock", book.Stock);

            connection.Open();

            return command.ExecuteNonQuery() > 0;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            string query = "SELECT BookId, Title, Author, Stock FROM Books";

            using SqlCommand command =
                new SqlCommand(query, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Book book = new Book();

                book.BookId = Convert.ToInt32(reader["BookId"]);
                book.Title = reader["Title"].ToString();
                book.Author = reader["Author"].ToString();
                book.Stock = Convert.ToInt32(reader["Stock"]);

                books.Add(book);
            }

            return books;
        }

        public bool UpdateStock(int bookId, int stock)
        {
            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            string query =
                "UPDATE Books SET Stock = @Stock WHERE BookId = @BookId";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Stock", stock);
            command.Parameters.AddWithValue("@BookId", bookId);

            connection.Open();

            return command.ExecuteNonQuery() > 0;
        }

        public bool RemoveBook(int bookId)
        {
            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            string query =
                "DELETE FROM Books WHERE BookId = @BookId";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@BookId", bookId);

            connection.Open();

            return command.ExecuteNonQuery() > 0;
        }

        public bool IssueBook(int bookId, int memberId)
        {
            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            connection.Open();

            using SqlTransaction transaction =
                connection.BeginTransaction();

            try
            {
                string stockQuery =
                    "SELECT Stock FROM Books WHERE BookId = @BookId";

                using SqlCommand stockCommand =
                    new SqlCommand(stockQuery, connection, transaction);

                stockCommand.Parameters.AddWithValue("@BookId", bookId);

                object result = stockCommand.ExecuteScalar();

                if (result == null)
                {
                    transaction.Rollback();
                    return false;
                }

                int stock = Convert.ToInt32(result);

                if (stock <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                string issueQuery =
                    @"INSERT INTO IssuedBooks
              (BookId, MemberId, IssueDate, ReturnDate)
              VALUES
              (@BookId, @MemberId, @IssueDate, NULL)";

                using SqlCommand issueCommand =
                    new SqlCommand(issueQuery, connection, transaction);

                issueCommand.Parameters.AddWithValue("@BookId", bookId);
                issueCommand.Parameters.AddWithValue("@MemberId", memberId);
                issueCommand.Parameters.AddWithValue("@IssueDate", DateTime.Now);

                issueCommand.ExecuteNonQuery();

                string updateQuery =
                    @"UPDATE Books
              SET Stock = Stock - 1
              WHERE BookId = @BookId";

                using SqlCommand updateCommand =
                    new SqlCommand(updateQuery, connection, transaction);

                updateCommand.Parameters.AddWithValue("@BookId", bookId);

                updateCommand.ExecuteNonQuery();

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool ReturnBook(int issueId)
        {
            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            connection.Open();

            using SqlTransaction transaction =
                connection.BeginTransaction();

            try
            {
                string findQuery =
                    @"SELECT BookId
              FROM IssuedBooks
              WHERE IssueId = @IssueId
              AND ReturnDate IS NULL";

                using SqlCommand findCommand =
                    new SqlCommand(findQuery, connection, transaction);

                findCommand.Parameters.AddWithValue("@IssueId", issueId);

                object result = findCommand.ExecuteScalar();

                if (result == null)
                {
                    transaction.Rollback();
                    return false;
                }

                int bookId = Convert.ToInt32(result);

                string returnQuery =
                    @"UPDATE IssuedBooks
              SET ReturnDate = @ReturnDate
              WHERE IssueId = @IssueId";

                using SqlCommand returnCommand =
                    new SqlCommand(returnQuery, connection, transaction);

                returnCommand.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                returnCommand.Parameters.AddWithValue("@IssueId", issueId);

                returnCommand.ExecuteNonQuery();

                string stockQuery =
                    @"UPDATE Books
              SET Stock = Stock + 1
              WHERE BookId = @BookId";

                using SqlCommand stockCommand =
                    new SqlCommand(stockQuery, connection, transaction);

                stockCommand.Parameters.AddWithValue("@BookId", bookId);

                stockCommand.ExecuteNonQuery();

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}