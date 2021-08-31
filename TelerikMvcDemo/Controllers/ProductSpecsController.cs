using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;
using TelerikMvcDemo.Models;
using TelerikMvcDemo.Repositories;

namespace TelerikMvcDemo.Controllers
{
    public class ProductSpecsController : Controller
    {
        private readonly Repository _repository;

        public ProductSpecsController()
        {
            _repository = new Repository();
        }

        [HttpPost]
        public async Task<ActionResult> Get(int productID, [DataSourceRequest] DataSourceRequest request)
        {
            var product = await _repository.QueryAsync<ProductSpec>(condition: $"ProductID = {productID}");

            return Json(product.ToDataSourceResult(request));
        }
    }
}
