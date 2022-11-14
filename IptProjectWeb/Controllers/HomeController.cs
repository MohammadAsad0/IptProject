using IptProjectWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
                client.BaseAddress = new Uri("https://sportsequipmentapi.azurewebsites.net/api/borroweditem/");

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

        public IActionResult Returned(string id)
        {
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