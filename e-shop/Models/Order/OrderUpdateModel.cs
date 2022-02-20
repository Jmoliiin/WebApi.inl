namespace e_shop.Models.Order
{
    public class OrderUpdateModel
    {
        public OrderUpdateModel()
        {
        }

        public OrderUpdateModel(int id, int statusId)
        {
            Id = id;
            StatusId = statusId;
        }

        public int Id { get; set; }
        public int StatusId { get; set; }
    }
}
