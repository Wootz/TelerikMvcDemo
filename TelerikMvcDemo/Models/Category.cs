using System.Collections.Generic;
using System.Linq;

namespace TelerikMvcDemo.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public static IEnumerable<Category> Categories => new List<Category>
        {
            new Category{ CategoryID = 1, CategoryName = "類別1"},
            new Category{ CategoryID = 2, CategoryName = "類別2"},
            new Category{ CategoryID = 3, CategoryName = "類別3"}
        };

        public static Category GetCategory(int? id)
        {
            return Categories.SingleOrDefault(c => c.CategoryID == id);
        }
    }
}