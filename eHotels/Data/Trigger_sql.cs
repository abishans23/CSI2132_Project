namespace Data
{
    static class TriggerString
    {
        public static readonly string bookingconflict = @"CREATE OR REPLACE FUNCTION fn_check_booking_intersection()
                    RETURNS TRIGGER AS $$
                    BEGIN
                        IF EXISTS (
                            SELECT 1 FROM Booking
                            WHERE HotelId = NEW.HotelId 
                            AND Roomnumber = NEW.Roomnumber 
                            AND NEW.startDate <= endDate 
                            AND NEW.endDate >= startDate 
                            AND status = 'Scheduled'
                        ) 
                        OR EXISTS (
                            SELECT 1 FROM Renting
                            WHERE HotelId = NEW.HotelId 
                            AND Roomnumber = NEW.Roomnumber 
                            AND NEW.startDate <= endDate 
                            AND NEW.endDate >= startDate 
                            AND status = 'Occupied'
                        )
                        THEN
                            RAISE EXCEPTION 'Booking time intersects another persons booking time'
                            USING ERRCODE = '45000';
                        END IF;

                        RETURN NEW;
                    END;
                    $$ LANGUAGE plpgsql;

                    DROP TRIGGER IF EXISTS checkIntersection ON Booking;

                    CREATE TRIGGER checkIntersection
                    BEFORE INSERT OR UPDATE ON Booking 
                    FOR EACH ROW
                    EXECUTE FUNCTION fn_check_booking_intersection();
                    ";
        
        public static readonly string rentingconflict = @"CREATE OR REPLACE FUNCTION fn_check_renting_intersection()
                    RETURNS TRIGGER AS $$
                    BEGIN
                        IF EXISTS (
                            SELECT 1 FROM Booking
                            WHERE HotelId = NEW.HotelId 
                            AND Roomnumber = NEW.Roomnumber 
                            AND NEW.startDate <= endDate 
                            AND NEW.endDate >= startDate 
                            AND status = 'Scheduled'
                        ) 
                        OR EXISTS (
                            SELECT 1 FROM Renting
                            WHERE HotelId = NEW.HotelId 
                            AND Roomnumber = NEW.Roomnumber 
                            AND NEW.startDate <= endDate 
                            AND NEW.endDate >= startDate 
                            AND status = 'Occupied'
                        )
                        THEN
                            RAISE EXCEPTION 'Renting time intersects another persons renting time'
                            USING ERRCODE = '45000';
                        END IF;

                        RETURN NEW;
                    END;
                    $$ LANGUAGE plpgsql;

                    DROP TRIGGER IF EXISTS checkIntersection ON Renting;

                    CREATE TRIGGER checkIntersection
                    BEFORE INSERT ON Renting 
                    FOR EACH ROW
                    EXECUTE FUNCTION fn_check_renting_intersection();
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
                    DROP TRIGGER IF EXISTS deleteBooking ON Renting;
                    CREATE TRIGGER deleteBooking
                    AFTER INSERT ON Renting
                    FOR EACH ROW
                    EXECUTE FUNCTION archive_and_delete_booking();
                    ";
    }
}