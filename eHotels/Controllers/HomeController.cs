using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eHotels.Models;
using Data;

namespace eHotels.Controllers;

public class HomeController : Controller
{
    private DBContext _db;

    public HomeController(DBContext db) 
    {
        _db = db; 
    }

    public async Task<IActionResult> Index()
    {
        //TODO::remove since only debug code
        //TODO::fix performance issue
        await _db.OpenConnection();
        return View();
    }


    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult Search(string search, string area, string capacity, string startDate, string endDate)
    {
        Console.WriteLine(search + area + capacity + startDate + endDate);
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(string username, string password, string email, string action)
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
