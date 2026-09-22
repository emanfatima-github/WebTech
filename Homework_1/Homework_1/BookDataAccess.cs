using Homework_1;
using System;
using System.Collections.Generic;
using System.IO;

namespace Homework_1
{
    public class BookDataAccess
    {
        private string filePath = "books.txt";

        // Add a book to the file
        public void AddBook(Book book)
        {
            FileStream fileStream = new FileStream(
                filePath,
                FileMode.Append
            );

            StreamWriter writer = new StreamWriter(fileStream);

            writer.WriteLine(
                $"{book.Id},{book.Title},{book.Author},{book.Price}"
            );

            writer.Close();
            fileStream.Close();
        }

        // Read all books from the file
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            if (!File.Exists(filePath))
            {
                return books;
            }

            FileStream fileStream = new FileStream(
                filePath,
                FileMode.Open
            );

            StreamReader reader = new StreamReader(fileStream);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(',');

                Book book = new Book
                {
                    Id = int.Parse(parts[0].Trim()),
                    Title = parts[1].Trim(),
                    Author = parts[2].Trim(),
                    Price = double.Parse(parts[3].Trim())
                };

                books.Add(book);
            }

            reader.Close();
            fileStream.Close();

            return books;
        }

        // Find a book by ID
        public Book FindBookById(int id)
        {
            List<Book> books = GetAllBooks();

            foreach (Book book in books)
            {
                if (book.Id == id)
                {
                    return book;
                }
            }

            return null;
        }

        // Create backup using byte-oriented file copying
        public void CreateBackup()
        {
            string backupPath = "books_backup.txt";

            FileStream sourceStream = new FileStream(
                filePath,
                FileMode.Open
            );

            FileStream destinationStream = new FileStream(
                backupPath,
                FileMode.Create
            );

            byte[] buffer = new byte[1024];

            int bytesRead;

            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }

            sourceStream.Close();
            destinationStream.Close();
        }
    }
}