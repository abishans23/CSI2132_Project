namespace Data
{
    static class ViewString
    {
        public static readonly string RoomNum = @"CREATE VIEW RoomNum AS
                    SELECT hotelid, COUNT(*) AS room_count
                    FROM Room
                    GROUP BY hotelid;";

        public static readonly string RoomNumCity = @"CREATE VIEW RoomNumCity AS
                    SELECT city, COUNT(*)  FROM (Address NATURAL JOIN Hotel NATURAL JOIN Room)
                    GROUP BY city;";
    }
}