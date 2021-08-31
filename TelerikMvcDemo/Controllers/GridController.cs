using Kendo.Mvc.UI;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TelerikMvcDemo.Models;
using TelerikMvcDemo.Repositories;

namespace TelerikMvcDemo.Controllers
{
    public partial class GridController : Controller
    {
        private readonly Repository _repository;

        public GridController()
        {
            _repository = new Repository();
        }

        public async Task<ActionResult> LocalBinding([DataSourceRequest] DataSourceRequest request)
        {
            var pageIndex = request?.Page;
            var pageSize = request?.PageSize;

            var sortDescriptor = request?.Sorts?.FirstOrDefault();
            var sort = sortDescriptor?.Member;
            var desc = sortDescriptor?.SortDirection == ListSortDirection.Descending;

            var data = await _repository.QueryAsync<Product>(pageIndex, pageSize, sort, desc);
            var total = await _repository.CountAsync<Product>();

            ViewBag.Total = total;

            return View(data);
        }

        public ActionResult RemoteBinding()
        {
            return View();
        }

        public ActionResult WebApiBinding()
        {
            return View();
        }

        public ActionResult BatchEditing()
        {
            return View();
        }

        public ActionResult InlineEditing()
        {
            return View();
        }

        public ActionResult PopupEditing()
        {
            return View();
        }
    }
}
