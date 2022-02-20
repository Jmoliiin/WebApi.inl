namespace e_shop.Models.Helpers
{
    public class ProductAndInventoryModel
    {
        public ProductAndInventoryModel()
        {
        }

        public ProductAndInventoryModel(string articleNumber, string productName, string description, decimal price, int quantity)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public ProductAndInventoryModel(int id, string articleNumber, string productName, string description, decimal price, int quantity)
        {
            Id = id;
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public string ArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
