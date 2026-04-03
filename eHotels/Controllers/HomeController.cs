using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eHotels.Models;
using Data;
using Npgsql;

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
    public async Task<IActionResult> SignIn(string username, string password, string emailAddress, string action)
    {

        ModelState.Clear();
        Console.WriteLine(emailAddress + " " + username + " " + password + " " + action);

        if (username == null || password == null || emailAddress == null){
            ModelState.AddModelError("", "username, password and email can not be empty");
            return View("SignIn");
        }

        if (action == "Login")
        {   
            var queryResult = await _db.QueryAsync<Account>("SELECT * From Account Where Email = @findEmail", new{findEmail=emailAddress});
            var accountsFound = queryResult.ToList();

            if (accountsFound.Count < 1)
            {
                ModelState.AddModelError("", "account does not exist...");
                return View("SignIn");
            }

            var accountInfo = accountsFound[0];

            if (emailAddress != accountInfo.Email || username != accountInfo.Username || password != accountInfo.Password)
            {
                return View("SignIn");
            }

            return View("Index");
        }

        if (action == "SignIn")
        {
            
            

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
