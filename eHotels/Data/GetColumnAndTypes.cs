namespace Data
{
    static class ColumnsAndTypes
    {
        public static readonly string GetHotel = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotel' ORDER BY ordinal_position;";
        
        public static readonly string GetHotelChain = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelchain' ORDER BY ordinal_position;";
        
        public static readonly string GetAddress = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'address' ORDER BY ordinal_position;";

        public static readonly string GetHotelEmail = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelemail' ORDER BY ordinal_position;";

        public static readonly string GetHotelPhone = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelphone' ORDER BY ordinal_position;";

        public static readonly string GetHotelChainEmail = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelchainemail' ORDER BY ordinal_position;";

        public static readonly string GetHotelChainPhone = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelchainphone' ORDER BY ordinal_position;";

        public static readonly string GetHotelImage = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelimage' ORDER BY ordinal_position;";

        public static readonly string GetHotelAmenity = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'hotelamenity' ORDER BY ordinal_position;";

        public static readonly string GetAccount = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'account' ORDER BY ordinal_position;";

        public static readonly string GetEmployee = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'employee' ORDER BY ordinal_position;";

        public static readonly string GetRoom = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'room' ORDER BY ordinal_position;";

        public static readonly string GetRoomProblem = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'roomproblem' ORDER BY ordinal_position;";

        public static readonly string GetRoomAmenity = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'roomamenity' ORDER BY ordinal_position;";

        public static readonly string GetBooking = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'booking' ORDER BY ordinal_position;";


        public static readonly string GetCustomer = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'customer' ORDER BY ordinal_position;";


        public static readonly string GetRenting = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'renting' ORDER BY ordinal_position;";

        public static readonly string GetReview = @"SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'review' ORDER BY ordinal_position;";
    }
}