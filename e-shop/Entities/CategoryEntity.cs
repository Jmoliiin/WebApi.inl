using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    [Index(nameof(CategoryName), IsUnique = true)]
    public class CategoryEntity
    {
        public CategoryEntity(string categoryName)
        {
            CategoryName = categoryName;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(50)")]
        public string CategoryName { get; set; }



        //skapa relation till Product
        public ICollection<ProductsEntity> Products { get; set; }
    }
}
