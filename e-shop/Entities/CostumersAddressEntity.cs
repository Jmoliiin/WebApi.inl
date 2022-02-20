using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    public class CostumersAddressEntity
    {
        public CostumersAddressEntity(string streetName, string postalCode, string city, string country)
        {
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string StreetName { get; set; }

        [Required]
        [Column(TypeName = "char(5)")]
        public string PostalCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }


        //Adress, skapa relation till costumers
        public ICollection<CostumersEntity> Costumers { get; set; }
    }
}
