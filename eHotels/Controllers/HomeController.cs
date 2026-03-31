using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eHotels.Models;
using Data;

namespace eHotels.Controllers;

public class HomeController : Controller
{


    public HomeController() 
    {
    }

    public async Task<IActionResult> Index()
    {
        //TODO::remove since only debug code
        //TODO::fix performance issue
        await DBContext.getInstance().OpenConnection();
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
