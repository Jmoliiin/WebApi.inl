using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    public class PassWordsEntity
    {
        public PassWordsEntity(string password)
        {
            Password = password;
        }

        [Key]
        public int Id { get; set; }

        //Här ska Hash och salt implementeras istället för password
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; set; }

        //Relation till Costumer
        public ICollection<CostumersEntity> Costumers { get; set; }
    }
}
