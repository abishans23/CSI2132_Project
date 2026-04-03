using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eHotels.Models;

namespace eHotels.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult Search(String search, String area, String capacity, String startDate, String endDate)
    {
        Console.WriteLine(search + area + capacity + startDate + endDate);
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(String username, String password, String action)
    {
        Console.WriteLine(username + " " + password + " " + action);
        ModelState.Clear();

        if (username == null || password == null){
            ModelState.AddModelError("", "username and password can't be empty");
            return View("SignIn");
        }

        if (true)
        {
            ModelState.AddModelError("", "u dont exist :(");
            return View("SignIn");
        }

        return View("Index");
    }

    public IActionResult CheckIn()
    {
        return View();
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
