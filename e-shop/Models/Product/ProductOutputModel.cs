namespace e_shop.Models.Product
{
    public class ProductOutputModel
    {
        public ProductOutputModel()
        {
        }

        public ProductOutputModel(int id, string articleNumber, string productName, string description, decimal price, int categoryId, string categoryName, int inventoryId, int inventoryQuantity, DateTime created)
        {
            Id = id;
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            CategoryName = categoryName;
            InventoryId = inventoryId;
            InventoryQuantity = inventoryQuantity;
            Created = created;
        }

        public ProductOutputModel(int id, string articleNumber, string productName, string description, decimal price, int categoryId, string categoryName, int inventoryId, int inventoryQuantity, DateTime created, DateTime updated)
        {
            Id = id;
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            CategoryName = categoryName;
            InventoryId = inventoryId;
            InventoryQuantity = inventoryQuantity;
            Created = created;
            Modified = updated;
        }

        public int Id { get; set; }
        public string ArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int InventoryId { get; set; }
        public int InventoryQuantity { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
