using System.ComponentModel.DataAnnotations;

namespace e_shop.Entities
{
    public class ProductInventoryEntity
    {
        public ProductInventoryEntity(int quantity)
        {
            Quantity = quantity;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }



        //skapa relation till product
        public ICollection<ProductsEntity> Products { get; set; }
    }
}
