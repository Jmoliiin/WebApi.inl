namespace e_shop.Models.Helpers
{
    public class OrderRowModel
    {
        public OrderRowModel(int orderId, int productId, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
