using Homework_1;
using System;
using System.Collections.Generic;

namespace Homework_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookDataAccess dataAccess = new BookDataAccess();

            int choice = 0;

            while (choice != 5)
            {
                Console.WriteLine();
                Console.WriteLine("===== BOOK MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Find Book by ID");
                Console.WriteLine("4. Create Backup");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");

                choice = int.Parse(Console.ReadLine());

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        AddBook(dataAccess);
                        break;

                    case 2:
                        ViewAllBooks(dataAccess);
                        break;

                    case 3:
                        FindBook(dataAccess);
                        break;

                    case 4:
                        CreateBackup(dataAccess);
                        break;

                    case 5:
                        Console.WriteLine("Application terminated.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void AddBook(BookDataAccess dataAccess)
        {
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Book Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Author Name: ");
            string author = Console.ReadLine();

            Console.Write("Enter Price: ");
            double price = double.Parse(Console.ReadLine());

            Book book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Price = price
            };

            dataAccess.AddBook(book);

            Console.WriteLine("Book added successfully.");
        }

        static void ViewAllBooks(BookDataAccess dataAccess)
        {
            List<Book> books = dataAccess.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            Console.WriteLine("===== ALL BOOKS =====");

            foreach (Book book in books)
            {
                book.DisplayInfo();
            }
        }

        static void FindBook(BookDataAccess dataAccess)
        {
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());

            Book book = dataAccess.FindBookById(id);

            if (book != null)
            {
                Console.WriteLine("Book found:");
                book.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        static void CreateBackup(BookDataAccess dataAccess)
        {
            dataAccess.CreateBackup();

            Console.WriteLine("Backup created successfully.");
        }
    }
}
