namespace e_shop.Models.Category
{
    public class CategoryOutputModel
    {
        public CategoryOutputModel()
        {
        }

        public CategoryOutputModel(int id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName;
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
