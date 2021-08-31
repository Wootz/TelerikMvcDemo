using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using TelerikMvcDemo.Models;
using TelerikMvcDemo.Repositories;

namespace TelerikMvcDemo.ApiControllers
{
    public class ProductsController : ApiController
    {
        private readonly Repository _repository;

        public ProductsController()
        {
            _repository = new Repository();
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _repository.GetAllAsync<Product>();
        }

        [HttpGet]
        public async Task<Product> GetAsync(int id)
        {
            return await _repository.GetAsync<Product>(id);
        }

        [HttpPost]
        public async Task<DataSourceResult> PostAsync([FromBody] Product product)
        {
            await _repository.InsertAsync(product);

            return new DataSourceResult { Data = new[] { product } };
        }

        [HttpPut]
        public async Task<DataSourceResult> PutAsync([FromBody] Product product)
        {
            await _repository.UpdateAsync(product);

            return new DataSourceResult { Data = new[] { product } };
        }

        [HttpDelete]
        public async Task<DataSourceResult> DeleteAsync([FromBody] Product product)
        {
            await _repository.DeleteAsync(product);

            return new DataSourceResult { Data = new[] { product } };
        }

        [HttpGet]
        [Route("api/Products/Read")]
        public async Task<DataSourceResult> ReadAsync([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var pageIndex = request?.Page;
            var pageSize = request?.PageSize;

            var sortDescriptor = request?.Sorts?.FirstOrDefault();
            var sort = sortDescriptor?.Member;
            var desc = sortDescriptor?.SortDirection == ListSortDirection.Descending;

            var data = await _repository.QueryAsync<Product>(pageIndex, pageSize, sort, desc);
            var total = await _repository.CountAsync<Product>();

            return new DataSourceResult { Data = data, Total = total };
        }

        [HttpPost]
        [Route("api/Products/CreateRange")]
        public async Task<DataSourceResult> CreateRangeAsync([FromBody] IEnumerable<Product> products)
        {
            if (products.Any())
            {
                await _repository.InsertRangeAsync(products);
            }

            return new DataSourceResult { Data = products };
        }

        [HttpPut]
        [Route("api/Products/UpdateRange")]
        public async Task<DataSourceResult> UpdateRangeAsync([FromBody] IEnumerable<Product> products)
        {
            if (products.Any())
            {
                await _repository.UpdateRangeAsync(products);
            }

            return new DataSourceResult { Data = products };
        }

        [HttpDelete]
        [Route("api/Products/DeleteRange")]
        public async Task<DataSourceResult> DeleteRangeAsync([FromBody] IEnumerable<Product> products)
        {
            if (products.Any())
            {
                await _repository.DeleteRangeAsync(products);
            }

            return new DataSourceResult { Data = products };
        }

        [HttpGet]
        [Route("api/Products/GetCategories")]
        public IEnumerable<Category> GetCategories()
        {
            return Category.Categories;
        }

        [HttpGet]
        [Route("api/Products/GetSubCategories")]
        public IEnumerable<SubCategory> GetSubCategories(int categoryID)
        {
            return SubCategory.GetSubCategories(categoryID);
        }
    }
}