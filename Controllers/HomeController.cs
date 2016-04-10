using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Adnug_Docker_Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "ADNUG Presentation";
            ViewData["HostName"] = Environment.GetEnvironmentVariable("HOSTNAME") ??
                Environment.GetEnvironmentVariable("COMPUTERNAME");
            ViewData["Name"] = Environment.GetEnvironmentVariable("NAME") ?? "unknown";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
