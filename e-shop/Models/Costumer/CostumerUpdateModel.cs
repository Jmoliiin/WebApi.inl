namespace e_shop.Models.Costumer
{
    public class CostumerUpdateModel
    {
        public CostumerUpdateModel()
        {
        }

        public CostumerUpdateModel(int id, string firstName, string lastName, string email, string streetName, string postalCode, string city, string country)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
