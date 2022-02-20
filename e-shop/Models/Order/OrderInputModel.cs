namespace e_shop.Models.Order
{
    public class OrderInputModel
    {
        public OrderInputModel()
        {
        }

        public OrderInputModel(int costumersId, int statusId)
        {
            CostumersId = costumersId;
            StatusId = statusId;
        }

        public int CostumersId { get; set; }

        public int StatusId { get; set; }
    }
}
