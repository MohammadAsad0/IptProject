using IptProjectWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace IptProjectWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<BorrowedItem>? items;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7201/api/borroweditem/");

                var responseTask = client.GetAsync("getallborroweditems");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BorrowedItem>>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    items = Enumerable.Empty<BorrowedItem>();
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact Administrator.");
                }
            }

            return View(items);
        }

        public async Task<IActionResult> Returned(string ItemId, string StudentId)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7201/api/borroweditem/");

            var uri = Path.Combine("deleteborroweditembyitemidandstudentid", ItemId, StudentId);

            var response = await client.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("index");
        }

        public IActionResult Edit(string? ItemId, string ItemName, string StudentId, string StudentName, string QuantityBorrowed, string TimeBorrowed, string TimeToBeReturned)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BorrowedItem item, string itemId, string studentId)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7201/api/borroweditem/");

            var json = new
            {
                item.ItemId,
                item.StudentId,
                item.QuantityBorrowed,
                item.TimeBorrowed,
                item.TimeToBeReturned
            };

            Console.WriteLine(json);
            var jsonItem = JsonConvert.SerializeObject(json);

            var data = new StringContent(jsonItem, Encoding.UTF8, "application/json");

            var uri = Path.Combine("updateborroweditembyitemidandstudentid", item.ItemId, item.StudentId);

            var response = await client.PutAsync(uri, data);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}