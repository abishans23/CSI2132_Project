using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using eHotels.Models;
using Data;
using Npgsql;
using System.Net;

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

        Console.WriteLine("current user: " + HttpContext.Session.GetString("Email") + " " + HttpContext.Session.GetString("Username"));
        
        await _db.OpenConnection();

        string chainQuery = "SELECT Name FROM HotelChain";
        var chainsQueryResult = await _db.QueryAsync<string>(chainQuery);
        var chainsNames = chainsQueryResult.ToList();

        return View(chainsNames);
    }


    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.SetString("Email", "");
        HttpContext.Session.SetString("Username", "");
        return View("Index");
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

        string accountQuery = "SELECT * From Account Where Email = @findEmail";
        var queryResult = await _db.QueryAsync<Account>(accountQuery, new {findEmail=emailAddress});
        var accountList = queryResult.ToList();

        if (action == "Login")
        {
            if (accountList.Count < 1)
            {
                ModelState.AddModelError("", "account doesn't exist");
                return View("SignIn");
            }

            var accountInfo = accountList[0];

            if (accountInfo.Email != emailAddress || accountInfo.Username != username || accountInfo.Password != password)
            {
                ModelState.AddModelError("", "account doesn't exist");
                return View("SignIn");
            }

        }

        if (action == "SignUp")
        {
            if (accountList.Count > 0)
            {
                ModelState.AddModelError("", "account with email already exists!");
                return View("SignIn");
            }

            await _db.ExecuteAsync(@"INSERT INTO Account VALUES (@emailAddress, @username, @password)", new{emailAddress, username, password});

            return View("Index");
        }

        HttpContext.Session.SetString("Email", emailAddress);
        HttpContext.Session.SetString("Username", username);

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
