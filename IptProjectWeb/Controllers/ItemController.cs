using IptProjectWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Text;
using System.Text.Json;

namespace IptProjectWeb.Controllers
{
    public class ItemController : Controller
    {
        IEnumerable<Item>? items;

        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7201/api/items/");

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7201/api/items/");

            var json = new
            {
                item.Name,
                item.PricePerItem,
                item.Quantity
            };

            var jsonItem = JsonConvert.SerializeObject(json);

            var data = new StringContent(jsonItem, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("additem", data);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        
        public IActionResult Edit(string? id, string name, string Quantity, string pricePerItem)
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Item item)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7201/api/items/");

            var json = new
            {
                item.Name,
                item.PricePerItem,
                item.Quantity
            };

            Console.WriteLine(json);

            var jsonItem = JsonConvert.SerializeObject(json);

            var data = new StringContent(jsonItem, Encoding.UTF8, "application/json");

            var uri = Path.Combine("updateitembyitemid", item.Id);

            var response = await client.PutAsync(uri, data);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAsync(string? id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7201/api/items/");

            var uri = Path.Combine("deleteitembyitemid", id);

            var response = await client.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("index");
        }

        public IActionResult Issue(string Id, string Name, int Quantity, int PricePerItem) 
        {
            var item = new Item(Id, Name, Quantity, PricePerItem);

            ViewData["Item"] = item;
            return View(); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(BorrowedItem item, string itemId)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy");

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7201/api/borroweditem/");

            var json = new
            {
                item.ItemId,
                item.StudentId,
                item.QuantityBorrowed,
                timeBorrowed = date,
                item.TimeToBeReturned
            };

            Console.WriteLine(json);

            var jsonItem = JsonConvert.SerializeObject(json);

            var data = new StringContent(jsonItem, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("addborroweditem", data);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }
    }
}
