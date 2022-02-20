using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    public class StatusEntity
    {
        public StatusEntity()
        {
        }

        public StatusEntity(string statusType)
        {
            StatusType = statusType;
        }

        public StatusEntity(int id, string statusType)
        {
            Id = id;
            StatusType = statusType;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StatusType { get; set; }

        //Skapa relation till orders
        public ICollection<OrdersEntity> Orders { get; set; }
    }
}
