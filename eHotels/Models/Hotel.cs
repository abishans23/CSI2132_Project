namespace Data
{
    // Class must be public
    public class Hotel
    {
        // 1. All fields must be public properties with {get; set;}
        public int HotelID { get; set; }
        public int ChainID { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }
        public string Manager { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string ImageDesc { get; set; }

        // 2. Add a parameterless constructor
        public Hotel() { }

        public Hotel(int chainID, string name, int stars, string manager, string postalCode, string description, string fileName, string imageDesc)
        {
            this.ChainID = chainID;
            this.Name = name;
            this.Stars = stars;
            this.Manager = manager;
            this.PostalCode = postalCode;
            this.Description = description;
            this.FileName = fileName;
            this.ImageDesc = imageDesc;
        }

    }
}