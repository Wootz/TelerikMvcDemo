using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TelerikMvcDemo.Models;
using TelerikMvcDemo.Repositories;

namespace TelerikMvcDemo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Repository _repository;

        public ProductsController()
        {
            _repository = new Repository();
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var product = await _repository.GetAsync<Product>(id);

            return View(product);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _repository.GetAsync<Product>(id);

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(product);
            }

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var pageIndex = request?.Page;
            var pageSize = request?.PageSize;

            var sortDescriptor = request?.Sorts?.FirstOrDefault();
            var sort = sortDescriptor?.Member;
            var desc = sortDescriptor?.SortDirection == ListSortDirection.Descending;

            var data = await _repository.QueryAsync<Product>(pageIndex, pageSize, sort, desc);
            var total = await _repository.CountAsync<Product>();

            return Json(new DataSourceResult { Data = data, Total = total });
        }

        [HttpPost]
        public async Task<ActionResult> Create([DataSourceRequest] DataSourceRequest request, Product product)
        {
            if (product != null && ModelState.IsValid)
            {
                await _repository.InsertAsync(product);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> Update([DataSourceRequest] DataSourceRequest request, Product product)
        {
            if (product != null && ModelState.IsValid)
            {
                await _repository.UpdateAsync(product);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, Product product)
        {
            if (product != null && ModelState.IsValid)
            {
                await _repository.DeleteAsync(product);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> CreateRange([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Product> products)
        {
            if (products != null && ModelState.IsValid)
            {
                await _repository.InsertRangeAsync(products);
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateRange([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Product> products)
        {
            if (products != null && ModelState.IsValid)
            {
                await _repository.UpdateRangeAsync(products);
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRange([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Product> products)
        {
            if (products.Any())
            {
                await _repository.DeleteRangeAsync(products);
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }
    }
}
