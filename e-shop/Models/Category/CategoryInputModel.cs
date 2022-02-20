namespace e_shop.Models.Category
{
    public class CategoryInputModel
    {
        public CategoryInputModel()
        {
           
        }
        public CategoryInputModel(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }
    }
}
