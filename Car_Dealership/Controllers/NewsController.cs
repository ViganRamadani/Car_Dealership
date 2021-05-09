using Car_Dealership.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Controllers
{
    public class NewsController : Controller
    {
        private MongoClient client = new MongoClient("mongodb://127.0.0.1:27017/");

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(News news)
        {
            var database = client.GetDatabase("Car_Dealership");
            var table = database.GetCollection<News>("news");
            news.Id = Guid.NewGuid().ToString();
            table.InsertOne(news);
            ViewBag.Mgs = "News is Created";
            return View();
        }

        /* public IActionResult Create()
         {
             return View();
         }*/

    }
}
