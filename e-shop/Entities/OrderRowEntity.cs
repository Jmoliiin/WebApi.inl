using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    public class OrderRowEntity
    {
        public OrderRowEntity(int quantity, decimal totalPrice, int ordersId, int productsId)
        {
            Quantity = quantity;
            TotalPrice = totalPrice;
            OrdersId = ordersId;
            ProductsId = productsId;
        }

        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        public int OrdersId { get; set; }

        public int ProductsId { get; set; }

        public OrdersEntity Orders { get; set; }
        public ProductsEntity Products { get; set; }
    }
}
