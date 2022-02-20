namespace e_shop.Models.OrderRows
{
    public class OrderRowUpdateModel
    {
        public OrderRowUpdateModel(int id, int productsId, int quantity)
        {
            Id = id;
            ProductsId = productsId;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int ProductsId { get; set; }
        public int Quantity { get; set; }
    }
}
