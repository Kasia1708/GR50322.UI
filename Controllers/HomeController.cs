using System.Diagnostics;
using GR50322.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GR50322.UI.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            ViewData["text"]="Лабораторная работа №2";
            var listData = new List<ListDemo>
        {
            new ListDemo {Id=1, Name="Item 1"},
            new ListDemo {Id=2, Name="Item 2"},
            new ListDemo {Id=3, Name="Item 3"},
            new ListDemo {Id=4, Name="Item 4"}

        };
        var data = new SelectList(listData, "Id", "Name");
            return View(data);
        }

       
    }
    public class ListDemo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
