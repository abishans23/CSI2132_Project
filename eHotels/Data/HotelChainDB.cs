using System.Text.Json;
using Npgsql;

string content=File.ReadAllText("HotelChain.json");
await data=JsonSerializer.Deserialize<HotelChain>(content);
namespace Data
{
    
}



