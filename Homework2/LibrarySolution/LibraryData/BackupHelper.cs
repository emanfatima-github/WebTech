using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using LibraryData.Models;

namespace LibraryData
{
    public class BackupHelper
    {
        public void ExportBooks(List<Book> books, string filePath)
        {
            string json =
                JsonSerializer.Serialize(books,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(filePath, json);
        }

        public List<Book> ImportBooks(string filePath)
        {
            string json = File.ReadAllText(filePath);

            List<Book> books =
                JsonSerializer.Deserialize<List<Book>>(json);

            return books;
        }
    }
}