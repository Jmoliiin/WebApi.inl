namespace e_shop.Models.OrderRows
{
    public class OrderRowInputModel
    {
        public OrderRowInputModel(int orderID, int productId, int quantity)
        {
            OrderID = orderID;
            ProductId = productId;

            Quantity = quantity;

        }

        public int OrderID { get; set;}
        public int ProductId { get; set;}
        public int Quantity { get; set;}


    }
}
