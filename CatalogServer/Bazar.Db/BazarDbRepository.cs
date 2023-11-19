using CatalogServer.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CatalogServer.Bazar.Db
{
    public class BazarDbRepository
    {
        public Book RetriveBookById(int id)
        {
            var books = ReadCsvFile();
            return books.FirstOrDefault(book => book.BookId == id);
        }

        public Book EditStock(int bookId, int newStock)
        {
            var books = ReadCsvFile();

            var bookToEdit = books.FirstOrDefault(book => book.BookId == bookId);

            if (bookToEdit != null)
            {
                bookToEdit.Stock = newStock;
                WriteCsvFile(books);
            }

            return bookToEdit;
        }

        public List<BookSerachTopicDTO> RetriveBookByTopic(string topic)
        {
            var books = ReadCsvFile();

            return books
                .Where(book => book.Category.Equals(topic, StringComparison.OrdinalIgnoreCase))
                .Select(book => ToBookSearchDTO(book))
                .ToList();
        }

        private static BookSerachTopicDTO ToBookSearchDTO(Book book)
        {
            return new BookSerachTopicDTO() { Title = book.Title, Price = book.Price };
        }

        private List<Book> ReadCsvFile()
        {
            using (var reader = new StreamReader("books.csv"))
            using (
                var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture))
            )
            {
                return csv.GetRecords<Book>().ToList();
            }
        }

        private void WriteCsvFile(List<Book> books)
        {
            using (var writer = new StreamWriter("books.csv"))
            using (
                var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture))
            )
            {
                csv.WriteRecords(books);
            }
        }
    }
}
