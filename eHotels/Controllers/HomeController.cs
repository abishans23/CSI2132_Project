using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using eHotels.Models;
using Data;
using Npgsql;
using System.Net;
using System.ComponentModel.Design;
using System.Text.Json.Nodes;

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

        //aggregation query. Show the chains and their average stars across all hotels they own on front page
        string chainQuery = "SELECT ChainName, ROUND(AVG(Stars),2) as AvgStars FROM HotelChain NATURAL JOIN Hotel GROUP BY ChainID";
        var chainsQueryResult = await _db.QueryAsync<dynamic>(chainQuery);
        var chainsData = chainsQueryResult.ToList();

        return View(chainsData);
    }


    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult Manage()
    {
        string[] allTables = new string[] {"Account", "Address", "Archived Booking", "Archived Renting", "Booking",
            "Customer", "Employee", "Hotel", "Hotel Chain Email", "Hotel Chain Phone", "Hotel Email", "Hotel Phone",
            "Renting", "Review", "Room", "Room Amenity", "Room Problem"};
        
        ViewBag.TableNames = allTables;

        return View();
    }

    public async Task<IActionResult> GetTableRows(string tableName)
    {
        var tableDataQueryResult = await _db.QueryAsync<dynamic>(
                "SELECT * FROM " + tableName
            ); 
        
        var rows = tableDataQueryResult.ToList();

        return Json(new {rows});
    }

    public async Task<IActionResult> DeleteRow(string tableName, string primaryKeys)
    {
        Console.Write("RECIVED KEYS: ");
        Console.WriteLine(JsonNode.Parse(primaryKeys));
        //construct dynamic delete query given table name and primary key values
        var keys = JsonNode.Parse(primaryKeys).AsObject();

        string deleteQuery = "DELETE FROM " + tableName + " WHERE ";

        foreach (var k in keys)
        {
            deleteQuery += k.Key + " = '" + k.Value + "' AND ";
        }

        deleteQuery = deleteQuery.Remove(deleteQuery.Length - 4) + ";";

        Console.WriteLine(deleteQuery);

        await _db.ExecuteAsync(
                @deleteQuery
            );

        return Json(new{success=true});
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
        capacity = capacity == 0 ?  -1 : capacity;
        startDate = startDate == null ? "0001-01-01" : startDate;
        endDate = endDate == null ? "0001-01-01" : endDate;

        view = view == null ? "ANY" : view;
        minPrice = minPrice == null ? 0 : minPrice;
        maxPrice = maxPrice == null ? 99999 : maxPrice;
        minRoomCount = minRoomCount == null ? 0 : minRoomCount;
        maxRoomCount = maxRoomCount == null ? 999999 : maxRoomCount;
        stars = (stars == null || stars == 0) ? -1 : stars;

        Console.WriteLine(search + area + capacity + startDate + endDate);

        //run search query (nested query)
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

                new{
                    chainName=search, 
                    city=area,
                    view=view,
                    capacity=capacity,
                    minPrice=minPrice,
                    maxPrice=maxPrice,
                    minRoomCount=minRoomCount,
                    maxRoomCount=maxRoomCount,
                    stars=stars,
                    startDate=Convert.ToDateTime(startDate),
                    endDate=Convert.ToDateTime(endDate)
                }
            );

        var availableRooms = roomsQueryResult.ToList();
        var roomAmenities = new Dictionary<int, string>();

        foreach(var r in availableRooms)
        {
            var roomAmenityQueryResult = await _db.QueryAsync<string>(
                "SELECT amenity From RoomAmenity WHERE roomnumber = @currentRoomNumber AND hotelid = @currentHotelId",
                new{currentRoomNumber = r.roomnumber, currentHotelId=r.hotelid}
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

        if (username == null || password == null || emailAddress == null){
            ModelState.AddModelError("", "username, password and email can not be empty");
            return View("SignIn");
        }

        var queryResult = await _db.QueryAsync<Account>(
            "SELECT * From Account Where Email = @findEmail", 
            new {findEmail=emailAddress}
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
                new{emailAddress, username, password}
                );

        }

        HttpContext.Session.SetString("Email", emailAddress);
        HttpContext.Session.SetString("Username", username);

        return RedirectToAction("Index");
    }

    public async Task InsertAddress(int streetNumber, string streetName, string city, string province, string country, string postalCode)
    {
        await _db.ExecuteAsync(
            @"INSERT INTO Address VALUES (@streetNumber, @streetName, @postalCode, @province, @country, @city)", 
            new{streetNumber, streetName, postalCode, province, country, city}
            );
    }

    public async Task InsertCustomer(string idType, string idNumber, string firstName, string lastName, string postalCode, string phoneNumber, string? email)
    {
        var customerInsertResult = await _db.ExecuteAsync(
            @"INSERT INTO Customer VALUES (@idType, @idNumber, @firstName, @lastName, @registrationDate, @phoneNumber, @postalCode, @email)",
            new{
                idType, 
                idNumber, 
                firstName, 
                lastName, 
                registrationDate=DateTime.Now, 
                phoneNumber, 
                postalCode,
                email= email==null? "NULL" : email
                }
            );
    }

    public async Task InsertRenting(int hotelId, int roomNumber, string idType, string idNumber, 
        string startDate, string endDate, int amount, string payementMethod)
    {
        var rentingInsertResult = await _db.ExecuteAsync(
            @"INSERT INTO Renting (Status, StartDate, EndDate, PaymentMethod, Amount, ProcessedDate, RoomNumber, HotelId, IdType, IdNumber)" +
            "VALUES (@status, @startDate, @endDate, @paymentMethod, @amount, @proccessedDate, @roomNumber, @hotelId, @idType, @idNumber)",
            new{
                status="Occupied", 
                startDate=Convert.ToDateTime(startDate), 
                endDate=Convert.ToDateTime(endDate),
                paymentMethod=payementMethod, 
                amount,
                proccessedDate=DateTime.Now, 
                roomNumber, 
                hotelId, 
                idType, 
                idNumber
                }
            );
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(string idType, string idNumber, string firstName, string lastName, int streetNumber,
        string streetName, string city, string province, string country, string postalCode, string phoneNumber)
    {
        await InsertAddress(streetNumber, streetName, city, province, country, postalCode);
        await InsertCustomer(idType, idNumber, firstName, lastName, postalCode, phoneNumber, null);
        
        return RedirectToAction("CheckIn");
    }

    //insert the renting for an in-person check in
    [HttpPost]
    public async Task<IActionResult> InPersonCheckIn(int hotelId, int roomNumber, string idType, string idNumber, 
        string startDate, string endDate, int amount, string payementMethod)
    {    
        await InsertRenting(hotelId, roomNumber, idType, idNumber, startDate, endDate, amount, payementMethod);
        return RedirectToAction("CheckIn");
    }

    //insert booking for the customer in the DB. Also insert the customer into the DB.
    [HttpPost]
    public async Task<IActionResult> CreateBooking(int hotelId, int roomNumber, string idType, string idNumber, string firstName, 
        string lastName, int streetNumber, string streetName, string city, string province, string country, string postalCode,
        string startDate, string endDate, string phoneNumber)
    {
        await InsertAddress(streetNumber, streetName, city, province, country, postalCode);
        await InsertCustomer(idType, idNumber, firstName, lastName, postalCode, phoneNumber, HttpContext.Session.GetString("Email"));

        var bookingInsertResult = await _db.ExecuteAsync(
            @"INSERT INTO Booking (BookingDate, Status, StartDate, EndDate, RoomNumber, HotelId, IdType, IdNumber) 
                VALUES (@bookingDate, @status, @startDate, @endDate, @roomNumber, @hotelId, @idType, @idNumber)",
            new{
                bookingDate=DateTime.Now,
                status="Scheduled",
                startDate=Convert.ToDateTime(startDate),
                endDate=Convert.ToDateTime(endDate),
                roomNumber,
                hotelId,
                idType,
                idNumber
            }
        );

        return RedirectToAction("Search");
    }

    //find a customers booking at a a specified hotel
    public async Task<IActionResult> GetBooking(int hotelId, string idType, string idNumber)
    {
        var bookingQueryResult = await _db.QueryAsync<dynamic>(
            "SELECT * FROM BOOKING WHERE hotelid = @hotelId AND idtype = @idType AND idnumber = @idNumber;",
            new
            {
                hotelId,
                idType,
                idNumber
            });

        var foundBookings = bookingQueryResult.ToList();
        var BookingInfo = "";

        if (foundBookings.Count > 0)
        {
            var booking = foundBookings[0];
            BookingInfo = "Room number: " + booking.roomnumber + ", Start Date: " +  booking.startdate + ", End Date: " + booking.enddate;
        } else
        {
            BookingInfo = "No booking exists..";
        }

        return Json(new {BookingInfo});
    }

    public async Task<IActionResult> TransferBooking(int hotelId, string idType, string idNumber, string paymentMethod, int amount)
    {

        var bookingQueryResult = await _db.QueryAsync<dynamic>(
            "SELECT * FROM BOOKING WHERE hotelid = @hotelId AND idtype = @idType AND idnumber = @idNumber;",
            new
            {
                hotelId,
                idType,
                idNumber
            });

        var foundBookings = bookingQueryResult.ToList();

        if (foundBookings.Count < 1) {return View("CheckIn");}

        var booking = foundBookings[0];

        await InsertRenting(hotelId, booking.roomnumber, idType, idNumber, Convert.ToString(booking.startdate),  Convert.ToString(booking.enddate), amount, paymentMethod);

        return View("CheckIn");
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