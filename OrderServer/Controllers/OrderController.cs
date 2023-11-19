using Microsoft.AspNetCore.Mvc;
using OrderServer.Models;
using OrderServer.Utilites;
using System.Net.Http;
using System.Text.Json;

namespace OrderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderRepoistory OrderRepoistory { get; set; } = new OrderRepoistory();
        string path
        {
            get { return Environment.GetEnvironmentVariable("MYURL"); }
        }

        [HttpPost("Book/{BookID}")]
        public async Task<ActionResult> PurchuseBook([FromRoute] int BookID)
        {
            var getPath = path + $"/Book/{BookID}";
            var postPath = getPath;
            Console.WriteLine($"getPath is :{getPath}");
            Console.WriteLine($"postPath is :{postPath}");

            var response = Utility.FetchApiResponse(getPath);
            Console.WriteLine("Result it " + response);
            if (response == null)
            {
                return NotFound("Resource Not Found");
            }
            Book book = JsonSerializer.Deserialize<Book>(response);
            Console.WriteLine($"BookId is :{book.bookId}");

            if (Utility.CheckIfStockIsVaild(book))
            {
                HttpResponseMessage responseText = await Utility.SendPostApi(postPath, book);
                Console.WriteLine("PostApiResponseis:" + responseText.StatusCode);
                if (responseText.IsSuccessStatusCode)
                {
                    string responseContent = await responseText.Content.ReadAsStringAsync();
                    PurchaseBook(book);

                    return Created("Succefully Purchsed", book);
                }
                return BadRequest();
            }

            return Ok("Book is Not Available");
        }

        private void PurchaseBook(Book book)
        {
            var orders = OrderRepoistory.ReadCsvFile();
            Order order = new Order
            {
                BookId = book.bookId,
                date = DateOnly.FromDateTime(DateTime.Now)
            };
            if (orders == null)
            {
                order.OrderId = 1;
            }
            else
            {
                order.OrderId = orders.LastOrDefault().OrderId + 1;
            }

            orders.Add(order);
            OrderRepoistory.WriteCsvFile(orders);
        }
    }
}
