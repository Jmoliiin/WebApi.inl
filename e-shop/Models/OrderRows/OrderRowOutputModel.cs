namespace e_shop.Models.OrderRows
{
    public class OrderRowOutputModel
    {
        private decimal _totalprice;

        public OrderRowOutputModel(int id, int orderID, int productId, string productName, int quantity, decimal productPrice)
        {
            Id = id;
            OrderID = orderID;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            ProductPrice = productPrice;
            TotalPrice = _totalprice;
        }


        public int Id { get; set; }
        public int OrderID { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }

        public decimal TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = Quantity * ProductPrice; }
        }
    }
}
