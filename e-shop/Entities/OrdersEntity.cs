using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    public class OrdersEntity
    {
        public OrdersEntity(decimal totalPrice)
        {
            TotalPrice += totalPrice;
        }

        public OrdersEntity(int costumersId, int statusId, DateTime created)
        {
            CostumersId = costumersId;
            StatusId = statusId;
            Created = created;
        }



        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }

        [Required]
        public int CostumersId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Modified { get; set; }

        public CostumersEntity Costumers { get; set; }
        public StatusEntity Status { get; set; }

        //Skapa relation till orderrow
        public ICollection<OrderRowEntity> OrderRows { get; set; }
    }
}
