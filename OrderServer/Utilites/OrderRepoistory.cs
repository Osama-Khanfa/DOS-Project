using CsvHelper;
using CsvHelper.Configuration;
using OrderServer.Models;
using System.Globalization;

namespace OrderServer.Utilites
{
    public class OrderRepoistory
    {
        internal List<Order> ReadCsvFile()
        {
            using (var reader = new StreamReader("Order.Db/orders.csv"))
            using (
                var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture))
            )
            {
                return csv.GetRecords<Order>().ToList();
            }
        }

        internal void WriteCsvFile(List<Order> orders)
        {
            using (var writer = new StreamWriter("Order.Db/orders.csv"))
            using (
                var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture))
            )
            {
                csv.WriteRecords(orders);
            }
        }
    }
}
