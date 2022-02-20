namespace e_shop.Models.Product
{
    public class ProductInputModel
    {
        public ProductInputModel()
        {
        }

        public ProductInputModel(string articleNumber, string productName, string description, decimal price, string categoryName, int quantity)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            CategoryName = categoryName;
            Quantity = quantity;
        }



        public string ArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
    }
}
