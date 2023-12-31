using CsvHelper;
using CsvHelper.Configuration;
using OrderServer.Models;
using System.Globalization;

namespace OrderServer.Utilites
{
    public class OrderRepoistory
    {
        private readonly object _lock = new object();

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

        internal List<Order> ReadMasterCsvFile()
        {
            lock (_lock)
            {
                using (var reader = new StreamReader("MasterDb/orders.csv"))
                using (
                    var csv = new CsvReader(
                        reader,
                        new CsvConfiguration(CultureInfo.InvariantCulture)
                    )
                )
                {
                    return csv.GetRecords<Order>().ToList();
                }
            }
        }

        internal void WriteCsvMasterFile(List<Order> orders)
        {
            lock (_lock)
            {
                using (var writer = new StreamWriter("MasterDb/orders.csv"))
                using (
                    var csv = new CsvWriter(
                        writer,
                        new CsvConfiguration(CultureInfo.InvariantCulture)
                    )
                )
                {
                    csv.WriteRecords(orders);
                }
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
