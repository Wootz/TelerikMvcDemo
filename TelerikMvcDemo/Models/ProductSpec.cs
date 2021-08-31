using System.ComponentModel.DataAnnotations;
using DapperKey = Dapper.Contrib.Extensions.KeyAttribute;
using DapperTable = Dapper.Contrib.Extensions.TableAttribute;

namespace TelerikMvcDemo.Models
{
    [DapperTable("ProductSpecs")]
    public class ProductSpec
    {
        [DapperKey]
        public int ProductSpecID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public string SpecName { get; set; }

        public string Desctiption { get; set; }
    }
}