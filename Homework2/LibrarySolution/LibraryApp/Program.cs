using LibraryData;
using LibraryData.Models;
using LibraryData.Repositories;

namespace LibraryApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookRepository bookRepository = new BookRepository();
            MemberRepository memberRepository = new MemberRepository();
            BackupHelper backupHelper = new BackupHelper();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("==================================");
                Console.WriteLine("      LIBRARY MANAGEMENT SYSTEM");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Add Member");
                Console.WriteLine("4. View All Members");
                Console.WriteLine("5. Issue Book to Member");
                Console.WriteLine("6. Return Book");
                Console.WriteLine("7. Export Books to JSON");
                Console.WriteLine("8. Import Books from JSON");
                Console.WriteLine("9. Exit");
                Console.WriteLine("==================================");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook(bookRepository);
                        break;

                    case "2":
                        ViewBooks(bookRepository);
                        break;

                    case "3":
                        AddMember(memberRepository);
                        break;

                    case "4":
                        ViewMembers(memberRepository);
                        break;

                    case "5":
                        IssueBook(bookRepository);
                        break;

                    case "6":
                        ReturnBook(bookRepository);
                        break;

                    case "7":
                        ExportBooks(bookRepository, backupHelper);
                        break;

                    case "8":
                        ImportBooks(backupHelper);
                        break;

                    case "9":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void AddBook(BookRepository bookRepository)
        {
            Console.WriteLine("\n----- Add Book -----");

            Console.Write("Enter Book ID: ");
            int bookId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Author: ");
            string author = Console.ReadLine();

            Console.Write("Enter Stock: ");
            int stock = Convert.ToInt32(Console.ReadLine());

            Book book = new Book
            {
                BookId = bookId,
                Title = title,
                Author = author,
                Stock = stock
            };

            if (bookRepository.AddBook(book))
                Console.WriteLine("Book added successfully.");
            else
                Console.WriteLine("Book could not be added.");
        }

        static void ViewBooks(BookRepository bookRepository)
        {
            Console.WriteLine("\n----- All Books -----");

            List<Book> books = bookRepository.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            foreach (Book book in books)
            {
                Console.WriteLine(
                    $"ID: {book.BookId} | Title: {book.Title} | Author: {book.Author} | Stock: {book.Stock}");
            }
        }

        static void AddMember(MemberRepository memberRepository)
        {
            Console.WriteLine("\n----- Add Member -----");

            Console.Write("Enter Member ID: ");
            int memberId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Member Name: ");
            string name = Console.ReadLine();

            Member member = new Member
            {
                MemberId = memberId,
                Name = name
            };

            if (memberRepository.AddMember(member))
                Console.WriteLine("Member added successfully.");
            else
                Console.WriteLine("Member could not be added.");
        }

        static void ViewMembers(MemberRepository memberRepository)
        {
            Console.WriteLine("\n----- All Members -----");

            List<Member> members = memberRepository.GetAllMembers();

            if (members.Count == 0)
            {
                Console.WriteLine("No members found.");
                return;
            }

            foreach (Member member in members)
            {
                Console.WriteLine(
                    $"ID: {member.MemberId} | Name: {member.Name}");
            }
        }

        static void IssueBook(BookRepository bookRepository)
        {
            Console.WriteLine("\n----- Issue Book -----");

            Console.Write("Enter Book ID: ");
            int bookId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Member ID: ");
            int memberId = Convert.ToInt32(Console.ReadLine());

            if (bookRepository.IssueBook(bookId, memberId))
                Console.WriteLine("Book issued successfully.");
            else
                Console.WriteLine("Book could not be issued.");
        }

        static void ReturnBook(BookRepository bookRepository)
        {
            Console.WriteLine("\n----- Return Book -----");

            Console.Write("Enter Issue ID: ");
            int issueId = Convert.ToInt32(Console.ReadLine());

            if (bookRepository.ReturnBook(issueId))
                Console.WriteLine("Book returned successfully.");
            else
                Console.WriteLine("Book could not be returned.");
        }

        static void ExportBooks(
            BookRepository bookRepository,
            BackupHelper backupHelper)
        {
            Console.WriteLine("\n----- Export Books -----");

            List<Book> books = bookRepository.GetAllBooks();

            string filePath = "books.json";

            backupHelper.ExportBooks(books, filePath);

            Console.WriteLine("Books exported successfully.");
        }

        static void ImportBooks(BackupHelper backupHelper)
        {
            Console.WriteLine("\n----- Import Books -----");

            string filePath = "books.json";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("books.json was not found.");
                return;
            }

            List<Book> books = backupHelper.ImportBooks(filePath);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            foreach (Book book in books)
            {
                Console.WriteLine(
                    $"Title: {book.Title} | Stock: {book.Stock}");
            }
        }
    }
}