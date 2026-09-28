using System.Diagnostics;
using GR50322.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GR50322.UI.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

       
    }
}
