using System;
using System.ComponentModel.DataAnnotations;
using DapperKey = Dapper.Contrib.Extensions.KeyAttribute;
using DapperTable = Dapper.Contrib.Extensions.TableAttribute;
using DapperWrite = Dapper.Contrib.Extensions.WriteAttribute;
using DapperComputed = Dapper.Contrib.Extensions.ComputedAttribute;

namespace TelerikMvcDemo.Models
{
    [DapperTable("Products")]
    public class Product
    {
        [DapperKey]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; }

        [Display(Name = "單價")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "庫存數量")]
        public int UnitsInStock { get; set; }

        [Display(Name = "已停產")]
        public bool Discontinued { get; set; }

        [Display(Name = "最後供應日")]
        public DateTime? LastSupply { get; set; }

        [Display(Name = "已訂購數量")]
        public int UnitsOnOrder { get; set; }

        [UIHint("Category")]
        [Display(Name = "類別")]
        public int? CategoryID { get; set; }

        [Display(Name = "子類別")]
        public int? SubCategoryID { get; set; }

        public string QuantityPerUnit { get; set; }

        [DapperWrite(false)]
        [DapperComputed]
        public string CategoryName => Category.GetCategory(CategoryID)?.CategoryName ?? string.Empty;

        [DapperWrite(false)]
        [DapperComputed]
        public string SubCategoryName => SubCategory.GetCategory(SubCategoryID)?.SubCategoryName ?? string.Empty;
    }
}
