using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop.Entities
{
    [Index(nameof(ArticleNumber), IsUnique = true)]
    [Index(nameof(ProductName), IsUnique = true)]

    public class ProductsEntity
    {
        public ProductsEntity(string articleNumber, string productName, string description, decimal price)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
        }

        public ProductsEntity(string articleNumber, string productName, string description, decimal price, DateTime created)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            Created = created;
        }

        public ProductsEntity(string articleNumber, string productName, string description, decimal price, DateTime created, DateTime modified)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            Created = created;
            Modified = modified;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ArticleNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Modified { get; set; }



        //Product.
        //FK:Category
        //FK:Product Inventarie
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int ProductInventoryId { get; set; }


        public CategoryEntity Category { get; set; }
        public ProductInventoryEntity ProductInventory { get; set; }

        //Skapa relation till orderrows
        public ICollection<OrderRowEntity> OrderRows { get; set; }
    }
}
