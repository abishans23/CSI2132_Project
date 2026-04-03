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
    public IActionResult SignIn(String username, String password, string email, String action)
    {
        ModelState.Clear();
            Console.WriteLine(email + " " + username + " " + password + " " + action);

        if (username == null || password == null || email == null){
            ModelState.AddModelError("", "username, password and email can not be empty");
            return View("SignIn");
        }

        if (action == "Login")
        {
            ModelState.AddModelError("", "u dont exist :(");
            return View("SignIn");
        }

        // Account T = new Account(em)

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
