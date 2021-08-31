using System.Collections.Generic;
using System.Linq;

namespace TelerikMvcDemo.Models
{
    public class SubCategory
    {
        public int SubCategoryID { get; set; }

        public int CategoryID { get; set; }

        public string SubCategoryName { get; set; }

        public static IEnumerable<SubCategory> SubCategories => new List<SubCategory>
        {
            new SubCategory{ SubCategoryID = 1, CategoryID = 1, SubCategoryName = "類別1-1"},
            new SubCategory{ SubCategoryID = 2, CategoryID = 1, SubCategoryName = "類別1-2"},
            new SubCategory{ SubCategoryID = 3, CategoryID = 1, SubCategoryName = "類別1-3"},
            new SubCategory{ SubCategoryID = 4, CategoryID = 2, SubCategoryName = "類別2-1"},
            new SubCategory{ SubCategoryID = 5, CategoryID = 2, SubCategoryName = "類別2-2"},
            new SubCategory{ SubCategoryID = 6, CategoryID = 2, SubCategoryName = "類別2-3"},
            new SubCategory{ SubCategoryID = 7, CategoryID = 3, SubCategoryName = "類別3-1"},
            new SubCategory{ SubCategoryID = 8, CategoryID = 3, SubCategoryName = "類別3-2"},
            new SubCategory{ SubCategoryID = 9, CategoryID = 3, SubCategoryName = "類別3-3"}
        };

        public static SubCategory GetCategory(int? id)
        {
            return SubCategories.SingleOrDefault(c => c.SubCategoryID == id);
        }

        public static IEnumerable<SubCategory> GetSubCategories(int categoryId)
        {
            return SubCategories.Where(c => c.CategoryID == categoryId);
        }
    }
}