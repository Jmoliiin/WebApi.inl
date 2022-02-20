using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    //Make password uniq
    [Index(nameof(Email), IsUnique = true)]
    public class CostumersEntity
    {
        public CostumersEntity(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public CostumersEntity(string firstName, string lastName, string email, int passWordsId, int costumersAddressId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PassWordsId = passWordsId;
            CostumersAddressId = costumersAddressId;
        }



        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }



        
        [Required]
        public int PassWordsId { get; set; }

        [Required]
        public int CostumersAddressId { get; set; }

        public PassWordsEntity PassWords { get; set; }
        public CostumersAddressEntity CostumersAddress { get; set; }

        //Relation till orders
        public ICollection<OrdersEntity> Orders { get; set; }
    }
}
