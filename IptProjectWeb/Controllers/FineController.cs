using IptProjectWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace IptProjectWeb.Controllers
{
    public class FineController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Fine> fines;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7201/api/fine/");

                var responseTask = client.GetAsync("getallfines");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Fine>>();
                    readTask.Wait();

                    Console.WriteLine(readTask.Result.Count);

                    fines = readTask.Result;
                }
                else
                {
                    fines = Enumerable.Empty<Fine>();
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact Administrator.");
                }
            }

            Console.WriteLine(fines);
            return View(fines);
        }
    }
}
