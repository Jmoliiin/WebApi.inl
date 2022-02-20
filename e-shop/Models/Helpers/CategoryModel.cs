namespace e_shop.Models.Helpers
{
    public class CategoryModel
    {
        public CategoryModel()
        {
        }

        public CategoryModel(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }
    }
}
