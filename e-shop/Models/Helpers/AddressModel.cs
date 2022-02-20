namespace e_shop.Models.Helpers
{
    public class AddressModel
    {
        public AddressModel()
        {
        }

        public AddressModel(string streetName, string postalCode, string city, string country)
        {
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }

        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
