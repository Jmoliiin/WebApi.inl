namespace e_shop.Models.Order
{
    public class OrderAndListOfOrderRows
    {
        private decimal _totalPrice;

        public OrderAndListOfOrderRows(int id, int ordersId, int productsId, string productName, int quantity, decimal price)
        {
            Id = id;
            OrdersId = ordersId;
            ProductsId = productsId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            TotalPrice = _totalPrice;
        }

        public int Id { get; }
        public int OrdersId { get; }
        public int ProductsId { get; }
        public string ProductName { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = Quantity * Price; }
        }
    }
}
