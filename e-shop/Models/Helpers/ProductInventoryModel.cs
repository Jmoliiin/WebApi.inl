namespace e_shop.Models.Helpers
{
    public class ProductInventoryModel
    {
        public ProductInventoryModel()
        {
        }

        public ProductInventoryModel(int quantity)
        {
            Quantity = quantity;
        }

        public int Quantity { get; set; }
    }
}
