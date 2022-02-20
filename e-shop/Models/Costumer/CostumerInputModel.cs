namespace e_shop.Models.Costumer
{
    public class CostumerInputModel
    {
        public CostumerInputModel()
        {
        }

        public CostumerInputModel(string firstName, string lastName, string email, string password, string streetName, string postalCode, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
