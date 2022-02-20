namespace e_shop.Models.Product
{
    public class ProductUpdateModel
    {
        public ProductUpdateModel()
        {
        }

        public ProductUpdateModel(int id, string productName, string description, decimal price, string categoryName, int inventoryQuantity)
        {
            Id = id;
            ProductName = productName;
            Description = description;
            Price = price;
            CategoryName = categoryName;
            InventoryQuantity = inventoryQuantity;
        }

        public int Id { get; set; } 
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int InventoryQuantity { get; set; }
    }
}
