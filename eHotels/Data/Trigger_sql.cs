namespace Data
{
    static class TriggerString
    {
        public static readonly string bookingconflict = @"CREATE TRIGGER checkIntersection BEFORE INSERT ON Booking
                    FOR EACH ROW BEGIN
                    IF EXISTS(
                        SELECT 1 FROM Booking
                        WHERE NEW.HotelId = HotelId AND NEW.Roomnumber = Roomnumber AND NEW.startDate <= endDate AND NEW.endDate >= startDate status = ‘Scheduled’
                    )
                    OR
                    IF EXISTS(
                        SELECT 1 FROM Renting
                        WHERE NEW.HotelId = HotelId AND NEW.Roomnumber = Roomnumber AND NEW.startDate <= endDate AND NEW.endDate >= startDate AND status = ‘Active’
                    )

                    THEN SIGNAL SQLSTATE ‘45000’
                    SET MESSAGE TEXT ‘booking time intersects another persons booking time’;
                        END IF;
                    END;
                    ";
        
        public static readonly string rentingconflict = @"CREATE TRIGGER checkIntersection BEFORE INSERT ON Renting
                    FOR EACH ROW BEGIN
                    IF EXISTS(
                        SELECT 1 FROM Booking
                        WHERE NEW.HotelId = HotelId AND NEW.Roomnumber = Roomnumber AND NEW.startDate <= endDate AND NEW.endDate >= startDate AND status = ‘Scheduled’
                    )
                    OR
                    IF EXISTS(
                        SELECT 1 FROM Renting
                        WHERE NEW.HotelId = HotelId AND NEW.Roomnumber = Roomnumber AND NEW.startDate <= endDate AND NEW.endDate >= startDate AND status = ‘Active’
                    )

                    THEN SIGNAL SQLSTATE ‘45000’
                    SET MESSAGE TEXT ‘booking time intersects another persons booking time’;
                        END IF;
                    END;
                    ";

        public static readonly string deletebooking = @"CREATE OR REPLACE FUNCTION archive_and_delete_booking()
                    RETURNS TRIGGER AS $$
                    BEGIN
                        INSERT INTO ArchivedBooking (HotelId, BookingDate, Status, StartDate, EndDate)
                        SELECT HotelId, BookingDate, Status, StartDate, EndDate
                        FROM Booking
                        WHERE StartDate = NEW.StartDate 
                        AND EndDate = NEW.EndDate 
                        AND IdType = NEW.IdType 
                        AND IdNumber = NEW.IdNumber 
                        AND RoomNumber = NEW.RoomNumber 
                        AND HotelId = NEW.HotelId 
                        AND Status = 'Scheduled';

                        -- Delete the record from Booking
                        DELETE FROM Booking
                        WHERE StartDate = NEW.StartDate 
                        AND EndDate = NEW.EndDate 
                        AND IdType = NEW.IdType 
                        AND IdNumber = NEW.IdNumber 
                        AND RoomNumber = NEW.RoomNumber 
                        AND HotelId = NEW.HotelId;

                        RETURN NEW; -- Required for trigger functions
                    END;
                    
                    $$ LANGUAGE plpgsql;
                    CREATE TRIGGER deleteBooking
                    AFTER INSERT ON Renting
                    FOR EACH ROW
                    EXECUTE FUNCTION archive_and_delete_booking();
                    ";
    }
}