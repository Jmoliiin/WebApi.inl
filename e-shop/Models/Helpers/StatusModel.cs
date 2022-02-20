namespace e_shop.Models.Helpers
{
    public class StatusModel
    {
        public StatusModel()
        {
        }

        public StatusModel(string statusType)
        {
            StatusType = statusType;
        }

        public string StatusType { get; set; }
    }
}
