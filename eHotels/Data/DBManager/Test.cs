using System;
using Npgsql; // If this still has a squiggle, the build failed
using Microsoft.Extensions.Configuration;

class Test {
    static void Main() {
        var conn = new NpgsqlConnection();
        var builder = new ConfigurationBuilder();
        Console.WriteLine("Libraries are linked successfully!");
    }
}