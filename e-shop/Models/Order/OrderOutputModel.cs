using e_shop.Models.Helpers;

namespace e_shop.Models.Order
{
    public class OrderOutputModel
    {
        public OrderOutputModel()
        {
        }

        public OrderOutputModel(int id, decimal totalPrice, int statusId, string statusType, int costumersId, DateTime created, DateTime modified, AddressModel address)
        {
            Id = id;
            TotalPrice = totalPrice;
            StatusId = statusId;
            StatusType = statusType;
            CostumersId = costumersId;
            Created = created;
            Modified = modified;
            Address = address;
        }

        public OrderOutputModel(int id, decimal totalPrice, int statusId, string statusType, int costumersId, DateTime created, DateTime modified, AddressModel address, List<OrderAndListOfOrderRows> orderRows)
        {
            Id = id;
            TotalPrice = totalPrice;
            StatusId = statusId;
            StatusType = statusType;
            CostumersId = costumersId;
            Created = created;
            Modified = modified;
            Address = address;
            OrderRows = orderRows;
        }

        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public int StatusId { get; set; }
        public string StatusType { get; set; }

        public int CostumersId { get; set; }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
        public AddressModel Address { get; set; }

        public List<OrderAndListOfOrderRows> OrderRows { get; set; } =new List<OrderAndListOfOrderRows>();
    }
}
