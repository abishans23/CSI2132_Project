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

        string sql = await Utils.BuildUpdate("hotel", TableColumnsAndTypes.Hotel);
        Console.WriteLine(sql);

        string chainQuery = "SELECT ChainName FROM HotelChain";
        var chainsQueryResult = await _db.QueryAsync<string>(chainQuery);
        var chainsNames = chainsQueryResult.ToList();

        return View(chainsNames);
    }


    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult Manage()
    {
        return View();
    }
    public IActionResult LogOut()
    {
        HttpContext.Session.SetString("Email", "");
        HttpContext.Session.SetString("Username", "");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Search(string search, string area, int capacity, string startDate, string endDate,
        string? view, int? minPrice, int? maxPrice, int? minRoomCount, int? maxRoomCount, int? stars)
    {
        //set up default values for search query if not inserted by the user, we use 'ANY' in place for any null strings where 
        // any row is valid. Similar for -1 for integer values and 0001-01-01 for dates
        search = search == null ? "ANY" : search;
        area = area == null ? "ANY" : area;
        capacity = capacity == 0 ? -1 : capacity;
        startDate = startDate == null ? "0001-01-01" : startDate;
        endDate = endDate == null ? "0001-01-01" : endDate;

        view = view == null ? "ANY" : view;
        minPrice = minPrice == null ? 0 : minPrice;
        maxPrice = maxPrice == null ? 99999 : maxPrice;
        minRoomCount = minRoomCount == null ? 0 : minRoomCount;
        maxRoomCount = maxRoomCount == null ? 999999 : maxRoomCount;
        stars = (stars == null || stars == 0) ? -1 : stars;

        Console.WriteLine(search + area + capacity + startDate + endDate);

        //run search query
        var roomsQueryResult = await _db.QueryAsync<dynamic>(
                "SELECT * FROM Room r " +
                "JOIN Hotel h ON r.hotelid = h.hotelid " +
                "JOIN HotelChain hc ON h.chainid = hc.chainid " +
                "JOIN Address a ON h.postalCode = a.postalCode " +
                "JOIN RoomNum rn ON h.hotelid = rn.hotelid " +

                "WHERE (hc.chainname = @chainName OR @chainName = 'ANY') AND " +
                "(a.city = @city OR @city = 'ANY') AND " +
                "(r.view = @view OR @view = 'ANY') AND " +
                "(r.capacity = @capacity OR @capacity = -1) AND " +
                "(@minPrice <= r.price AND r.price <= @maxPrice) AND " +
                "(@minRoomCount <= rn.room_count AND rn.room_count <= @maxRoomCount) AND " +
                "(h.stars = @stars OR @stars = -1) AND " +

                "(NOT EXISTS (" +
                    "SELECT * FROM Booking b " +
                    "WHERE b.roomnumber = r.roomnumber " +
                    "AND @startDate <= b.EndDate " +
                    "AND @endDate >= b.StartDate " +
                ") OR @startDate = '0001-01-01') " +

                "AND (NOT EXISTS ( " +
                    "SELECT * FROM Renting rt " +
                    "WHERE rt.roomnumber = r.roomnumber " +
                    "AND @startDate <= rt.EndDate " +
                    "AND @endDate >= rt.StartDate " +
                ") OR @endDate = '0001-01-01');",

                new
                {
                    chainName = search,
                    city = area,
                    view = view,
                    capacity = capacity,
                    minPrice = minPrice,
                    maxPrice = maxPrice,
                    minRoomCount = minRoomCount,
                    maxRoomCount = maxRoomCount,
                    stars = stars,
                    startDate = Convert.ToDateTime(startDate),
                    endDate = Convert.ToDateTime(endDate)
                }
            );

        var availableRooms = roomsQueryResult.ToList();
        var roomAmenities = new Dictionary<int, string>();

        foreach (var r in availableRooms)
        {
            // Console.WriteLine(r);
            var roomAmenityQueryResult = await _db.QueryAsync<string>(
                "SELECT amenity From RoomAmenity WHERE roomnumber = @currentRoomNumber AND hotelid = @currentHotelId",
                new { currentRoomNumber = r.roomnumber, currentHotelId = r.hotelid }
            );

            var roomAmenitiesList = roomAmenityQueryResult.ToList();
            string roomAmenitiesString = roomAmenitiesList.Count > 0 ? roomAmenitiesList[0] : "";

            for (var i = 1; i < roomAmenitiesList.Count; i++)
            {
                roomAmenitiesString += ", " + roomAmenitiesList[i];
            }

            roomAmenities[r.roomnumber] = roomAmenitiesString;

        }

        ViewBag.availableRooms = availableRooms;
        ViewBag.roomAmenities = roomAmenities;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(string username, string password, string emailAddress, string action)
    {
        ModelState.Clear();
        Console.WriteLine(emailAddress + " " + username + " " + password + " " + action);

        if (username == null || password == null || emailAddress == null)
        {
            ModelState.AddModelError("", "username, password and email can not be empty");
            return View("SignIn");
        }

        var queryResult = await _db.QueryAsync<Account>(
            "SELECT * From Account Where Email = @findEmail",
            new { findEmail = emailAddress }
            );
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

            await _db.ExecuteAsync(
                @"INSERT INTO Account VALUES (@emailAddress, @username, @password)",
                new { emailAddress, username, password }
                );

        }

        HttpContext.Session.SetString("Email", emailAddress);
        HttpContext.Session.SetString("Username", username);

        return RedirectToAction("Index");
    }

    public async Task InsertAddress(int streetNumber, string streetName, string province, string country, string postalCode)
    {
        await _db.ExecuteAsync(
            @"INSERT INTO Address VALUES (@streetNumber, @streetName, @postalCode, @province, @country)",
            new { streetNumber, streetName, postalCode, province, country }
            );
    }

    public async Task InsertCustomer(string idType, string idNumber, string firstName, string lastName, string postalCode, string phoneNumber, string? email)
    {
        var customerInsertResult = await _db.ExecuteAsync(
            @"INSERT INTO Customer VALUES (@idType, @idNumber, @firstName, @lastName, @registrationDate, @phoneNumber, @postalCode, @email)",
            new
            {
                idType,
                idNumber,
                firstName,
                lastName,
                registrationDate = DateTime.Now,
                phoneNumber,
                postalCode,
                email = email == null ? "NULL" : email
            }
            );
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(string idType, string idNumber, string firstName, string lastName, int streetNumber,
        string streetName, string province, string country, string postalCode, string phoneNumber)
    {
        await InsertAddress(streetNumber, streetName, province, country, postalCode);
        await InsertCustomer(idType, idNumber, firstName, lastName, postalCode, phoneNumber, null);

        return RedirectToAction("CheckIn");
    }

    [HttpPost]
    public async Task<IActionResult> InPersonCheckIn(int hotelId, int roomNumber, string idType, string idNumber,
        string startDate, string endDate, int amount, string payementMethod)
    {
        var rentingInsertResult = await _db.ExecuteAsync(
            @"INSERT INTO Renting (Status, StartDate, EndDate, PaymentMethod, Amount, ProcessedDate, RoomNumber, HotelId, IdType, IdNumber)" +
            "VALUES (@status, @startDate, @endDate, @paymentMethod, @amount, @proccessedDate, @roomNumber, @hotelId, @idType, @idNumber)",
            new
            {
                status = "Occupied",
                startDate = Convert.ToDateTime(startDate),
                endDate = Convert.ToDateTime(endDate),
                paymentMethod = payementMethod,
                amount,
                proccessedDate = DateTime.Now,
                roomNumber,
                hotelId,
                idType,
                idNumber
            }
            );

        if (rentingInsertResult != 1)
        {
            Console.WriteLine("Error inserting renting");
        }

        return RedirectToAction("CheckIn");
    }

    //insert booking for the customer in the DB. Also insert the customer into the DB.
    [HttpPost]
    public async Task<IActionResult> CreateBooking(int hotelId, int roomNumber, string idType, string idNumber, string firstName,
        string lastName, int streetNumber, string streetName, string city, string province, string country, string postalCode,
        string startDate, string endDate, string phoneNumber)
    {
        await InsertAddress(streetNumber, streetName, province, country, postalCode);
        await InsertCustomer(idType, idNumber, firstName, lastName, postalCode, phoneNumber, HttpContext.Session.GetString("Email"));

        var bookingInsertResult = await _db.ExecuteAsync(
            @"INSERT INTO Booking (BookingDate, Status, StartDate, EndDate, RoomNumber, HotelId, IdType, IdNumber) 
                VALUES (@bookingDate, @status, @startDate, @endDate, @roomNumber, @hotelId, @idType, @idNumber)",
            new
            {
                bookingDate = DateTime.Now,
                status = "booked",
                startDate = Convert.ToDateTime(startDate),
                endDate = Convert.ToDateTime(endDate),
                roomNumber,
                hotelId,
                idType,
                idNumber
            }
        );

        return View("Search");
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