namespace e_shop.Models.Helpers
{
    public class ProductModel
    {
        public ProductModel()
        {
        }

        public ProductModel(string articleNumber, string productName, string description, decimal price)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
        }

        public string ArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
