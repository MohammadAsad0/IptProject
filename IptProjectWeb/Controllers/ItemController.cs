using IptProjectWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace IptProjectWeb.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Item> items = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://sportsequipmentapi.azurewebsites.net/api/items/");

                var responseTask = client.GetAsync("getallItems");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Item>>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    items = Enumerable.Empty<Item>();
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact Administrator.");
                }
            }

            return View(items);
        }
    }
}
