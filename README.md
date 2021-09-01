# Telerik UI for ASP.NET MVC（Kendo UI）
> Demo
> https://demos.telerik.com/aspnet-mvc

> 文件 Documentation
> https://docs.telerik.com/aspnet-mvc/introduction
> https://docs.telerik.com/kendo-ui/introduction

> 討論區 Forum
> https://www.telerik.com/forums/aspnet-mvc
> https://www.telerik.com/forums/kendo-ui

> 下載試用版
> https://www.telerik.com

新增Telerik UI for ASP.NET MVC專案
![](https://i.imgur.com/8kOaAkP.png)

# Grid
## Grid 資料來源
### DataSource
> [ASP.NET MVC API Documentation](https://docs.telerik.com/aspnet-mvc/api/datasource)
> [Javascript API Documentation](https://docs.telerik.com/kendo-ui/api/javascript/data/datasource)

* Local Binding
* Remote Binding
* Web Api Binding

### Local Binding
* Server()
* Total((int)ViewBag.Total)

> ~\Views\Grid\LocalBinding.cshtml
```
@(Html.Kendo().Grid<Product>(Model)
    .Name("grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.ProductID).Width(100);
        columns.Bound(p => p.ProductName);
        columns.Bound(p => p.CategoryID);
        columns.Bound(p => p.SubCategoryID);
        columns.Bound(p => p.UnitPrice).Width(100);
        columns.Bound(p => p.UnitsInStock).Width(100);
        columns.Bound(p => p.UnitsOnOrder).Width(100);
        columns.Bound(p => p.Discontinued).Sortable(false).Width(100);
        columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").Width(200);
    })
    .Pageable()
    .DataSource(dataSource => dataSource
        .Server()
        .Total((int)ViewBag.Total)
        .PageSize(10)
    )
)
```

> ~\Controllers\GridController.cs
```
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
}
```

### Remote Binding
* Ajax()
* Read(read => read.Action("Read", "Products"))
* 使用 POST 取得資料

> ~\Views\Grid\RemoteBinding.cshtml
```
@(Html.Kendo().Grid<Product>()
    .Name("grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.ProductID).Width(100);
        columns.Bound(p => p.ProductName);
        columns.Bound(p => p.CategoryID);
        columns.Bound(p => p.SubCategoryID);
        columns.Bound(p => p.UnitPrice).Width(100);
        columns.Bound(p => p.UnitsInStock).Width(100);
        columns.Bound(p => p.UnitsOnOrder).Width(100);
        columns.Bound(p => p.Discontinued).Sortable(false).Width(100);
        columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").Width(200);
    })
    .Pageable()
    .Sortable()
    .DataSource(dataSource => dataSource
        .Ajax()
        .PageSize(10)
        .Read(read => read.Action("Read", "Products"))
    )
)
```

> ~\Controllers\GridController.cs
```
public partial class GridController : Controller
{    
    public ActionResult RemoteBinding()
    {
        return View();
    }
}
```

> ~\Controllers\ProductsController.cs
```
public class ProductsController : Controller
{
    private readonly Repository _repository;

    public ProductsController()
    {
        _repository = new Repository();
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
}
```

### Web Api Binding
* WebApi()
* Read(read => read.Url("/api/Products/Read"))
* 使用 GET 取得資料

> ~\Views\Grid\WebApiBinding.cshtml
```
@(Html.Kendo().Grid<Product>()
    .Name("grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.ProductID).Width(100);
        columns.Bound(p => p.ProductName);
        columns.Bound(p => p.CategoryID);
        columns.Bound(p => p.SubCategoryID);
        columns.Bound(p => p.UnitPrice).Width(100);
        columns.Bound(p => p.UnitsInStock).Width(100);
        columns.Bound(p => p.UnitsOnOrder).Width(100);
        columns.Bound(p => p.Discontinued).Sortable(false).Width(100);
        columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").Width(200);
    })
    .Pageable()
    .Sortable()
    .DataSource(dataSource => dataSource
        .WebApi()
        .PageSize(10)
        .Read(read => read.Url("/api/Products/Read"))
    )
)
```

> ~\Controllers\GridController.cs
```
public partial class GridController : Controller
{    
    public ActionResult RemoteBinding()
    {
        return View();
    }
}
```

> ~\ApiControllers\ProductsController.cs
```
public class ProductsController : ApiController
{
    private readonly Repository _repository;

    public ProductsController()
    {
        _repository = new Repository();
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
}
```

## Grid 編輯模式
### ASP.NET MVC 相關
* Html.EditorFor(m => m.Feild, "Template_Name")
* ~/Views/Shared/EditorTemplates
> [認識View - DiaplayTempalte 與 EditorTemplates](https://ithelp.ithome.com.tw/articles/10161794)

### Inline Editing
* Editable(editable=> editable.Mode(GridEditMode.**InLine**))
* columns.Command(command => { command.Edit(); command.Destroy(); })
* ToolBar(toolbar => toolbar.Create())
* DataSource
    * Model(model => model.Id(p => p.ProductID))
      指定 ID 欄位
    * Read(read => read.Action("Read", "Products"))
    * Create(create => create.Action("Create", "Products"))
    * Update(update => update.Action("Update", "Products"))
    * Destroy(update => update.Action("Delete", "Products"))

> ~\Views\Grid\InlineEditing.cshtml
```
@(Html.Kendo().Grid<Product>()
    .Name("grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.ProductID).Width(100);
        columns.Bound(p => p.ProductName);
        columns.Bound(p => p.CategoryID);
        columns.Bound(p => p.SubCategoryID);
        columns.Bound(p => p.UnitPrice).Width(100);
        columns.Bound(p => p.UnitsInStock).Width(100);
        columns.Bound(p => p.UnitsOnOrder).Width(100);
        columns.Bound(p => p.Discontinued).Sortable(false).Width(100);
        columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").Width(200);
        columns.Command(command => {
            command.Edit().Text("編輯").UpdateText("更新").CancelText("取消");
            command.Destroy().Text("刪除");
        }).Width(250);
    })
    .ToolBar(toolbar => toolbar.Create().Text("新增"))
    .Pageable()
    .Sortable()
    .Editable(editable=> editable.Mode(GridEditMode.InLine))
    .DataSource(dataSource => dataSource
        .Ajax()
        .PageSize(10)
        .Model(model =>
        {
            model.Id(p => p.ProductID);
            model.Field(p => p.ProductID).Editable(false);
        })
        .Read(read => read.Action("Read", "Products"))
        .Create(create => create.Action("Create", "Products"))
        .Update(update => update.Action("Update", "Products"))
        .Destroy(update => update.Action("Delete", "Products"))
    )
)
```

> ~\Controllers\ProductsController.cs
```
public class ProductsController : Controller
{
    private readonly Repository _repository;

    public ProductsController()
    {
        _repository = new Repository();
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
}
```

### Batch Editing
* Editable(editable=> editable.Mode(GridEditMode.**InCell**))
* Batch(true)
* ToolBar(toolbar => { toolbar.Create(); toolbar.Save(); })
* DataSource
    * Read(read => read.Action("Read", "Products"))
    * Create(create => create.Action("CreateRange", "Products"))
    * Update(update => update.Action("UpdateRange", "Products"))
    * Destroy(update => update.Action("DeleteRange", "Products"))

> ~\Views\Grid\BatchEditing.cshtml
```
@(Html.Kendo().Grid<Product>()
    .Name("grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.ProductID).Width(100);
        columns.Bound(p => p.ProductName);
        columns.Bound(p => p.CategoryID);
        columns.Bound(p => p.SubCategoryID);
        columns.Bound(p => p.UnitPrice).Width(100);
        columns.Bound(p => p.UnitsInStock).Width(100);
        columns.Bound(p => p.UnitsOnOrder).Width(100);
        columns.Bound(p => p.Discontinued).Sortable(false).Width(100);
        columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").Width(200);
        columns.Command(command => command.Destroy().Text("刪除")).Width(100);
    })
    .ToolBar(toolbar => {
        toolbar.Create().Text("新增");
        toolbar.Save().Text("儲存").SaveText("儲存").CancelText("取消");
    })
    .Pageable()
    .Sortable()
    .Editable(editable=> editable.Mode(GridEditMode.InCell))
    .DataSource(dataSource => dataSource
        .Ajax()
        .Batch(true)
        .PageSize(10)
        .Model(model =>
        {
            model.Id(p => p.ProductID);
            model.Field(p => p.ProductID).Editable(false);
        })
        .Read(read => read.Action("Read", "Products"))
        .Create(create => create.Action("CreateRange", "Products"))
        .Update(update => update.Action("UpdateRange", "Products"))
        .Destroy(update => update.Action("DeleteRange", "Products"))
    )
)
```
* [Bind(Prefix = "models")]

> ~\Controllers\ProductsController.cs
```
public class ProductsController : Controller
{
    private readonly Repository _repository;

    public ProductsController()
    {
        _repository = new Repository();
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
```

### Popup Editing
* Editable(editable=> editable.Mode(GridEditMode.**PopUp**))

> ~\Views\Grid\PopupEditing.cshtml
```
@(Html.Kendo().Grid<Product>()
    .Name("grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.ProductID).Width(100);
        columns.Bound(p => p.ProductName);
        columns.Bound(p => p.CategoryID);
        columns.Bound(p => p.SubCategoryID);
        columns.Bound(p => p.UnitPrice).Width(100);
        columns.Bound(p => p.UnitsInStock).Width(100);
        columns.Bound(p => p.UnitsOnOrder).Width(100);
        columns.Bound(p => p.Discontinued).Sortable(false).Width(100);
        columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").Width(200);
        columns.Command(command => {
            command.Edit().Text("編輯").UpdateText("更新").CancelText("取消");
            command.Destroy().Text("刪除");
        }).Width(250);
    })
    .ToolBar(toolbar => toolbar.Create().Text("新增"))
    .Pageable()
    .Sortable()
    .Editable(editable=> editable.Mode(GridEditMode.PopUp))
    .DataSource(dataSource => dataSource
        .Ajax()
        .PageSize(10)
        .Model(model => {
            model.Id(p => p.ProductID);
            model.Field(p => p.ProductID).Editable(false);
        })
        .Read(read => read.Action("Read", "Products"))
        .Create(create => create.Action("Create", "Products"))
        .Update(update => update.Action("Update", "Products"))
        .Destroy(update => update.Action("Delete", "Products"))
    )
)
```

## Grid 進階
> [淺談 ASP.NET MVC 的生命週期](https://nwpie.blogspot.com/2017/05/5-aspnet-mvc.html)

> ~\Views\Grid\LocalBinding.cshtml

### 自定義表頭
```
columns.Bound(p => p.ProductName).HeaderTemplate("品名");
// OR
columns.Bound(p => p.ProductName).Title("品名");
```

### 自定義資料欄

#### 使用 Template()
* **適用於 Local Binding**
* 使用 Razor 語法

```
columns
    .Bound(p => p.ProductName)
    .Template(@<text>@item.ProductID - @item.ProductName</text>);
```

#### 使用 ClientTemplate()
* **適用於 Remote Binding**
* 使用 Kendo UI Template 語法
* https://docs.telerik.com/kendo-ui/framework/templates/overview
```
columns
    .Bound(p => p.Discontinued)
    .ClientTemplate("#= Discontinued ? '是' : '否'#");
```

##### 進階：使用 function
```
columns
    .Bound(p => p.ProductName)
    .ClientTemplate("#=productNameTemplate(data)#");
```

```
<script>
    function productNameTemplate(product) {
        if (product.Discontinued) {
            return '<a href="/Products/Details/' + product.ProductID + '" style="color: red;">' + product.ProductName + '</a>';
        }

        return '<a href="/Products/Details/' + product.ProductID + '">' + product.ProductName + '</a>';
    }
</script>
```

##### 進階：使用 Kendo UI Template
* #= text #
```
column
    .Bound(c => c.Sessions)
    .ClientTemplate("#= sessionsTemplate(data) #");
```

```
<script>
    var sessionsTemplate = kendo.template($('#sessions-template').html());
</script>
```

```
<script id="sessions-template" type="text/x-kendo-template">
    # var arr = []; #
    # for(var i = 0; i < Sessions.length; i++) { #
    #   var session = Sessions[i]; #
    #   arr.push(session.SessionCode + ' - ' + (session.SessionDisplayName || '(' + session.TermNo + ')' + session.SessionName)); #
    # } #
    # var text = arr.join('、'); #
    #= text #
</script>
```

### 自定義命令欄
```
columns.Command(command => {
    command.Custom("details").Text("明細").Click("details");
});
```

```
<script>
    function details(e) {
        e.preventDefault();

        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        var id = dataItem.ProductID;

        var url = '/Products/Details/' + id;

        window.location = url;
    }
</script>
```

### 自定義工具列
```
.ToolBar(toolbar => {
    toolbar.Create().Text("新增");
    toolbar.Custom().Text("回首頁").Action("Index", "Home");
})
```

### 下拉選單欄位連動
#### 準備 EditorTemplate
* Category 下拉選單
* SubCategory 下拉選單

##### Category 下拉選單
* 使用靜態資料

> ~\Views\Shared\EditorTemplates\Category.cshtml
```
@model int?

@{
    var onChange = ViewData["onChange"] as string;
    var htmlAttributes = ViewData["htmlAttributes"];
}

@(Html.Kendo().DropDownListFor(m => m)
    .DataValueField("CategoryID")
    .DataTextField("CategoryName")
    .OptionLabel("請選擇")
    .Events(events =>
    {
        if (string.IsNullOrEmpty(onChange) == false)
        {
            events.Change(onChange);
        }
    })
    .HtmlAttributes(htmlAttributes)
    .BindTo(Category.Categories)
)
```

##### SubCategory 下拉選單
* 依 Category 從伺服器取得選項

> ~\Views\Shared\EditorTemplates\SubCategory.cshtml
```
@model int?

@{
    var cascadeFrom = ViewData["cascadeFrom"] as string;
    var onChange = ViewData["onChange"] as string;
    var htmlAttributes = ViewData["htmlAttributes"];
}

<script>
    function filter(e) {
        var filters = e.filter.filters;
        if (!filters) {
            return {};
        }

        var result = {};
        for (var i = 0; i < filters.length; i++) {
            result[filters[i].field] = filters[i].value;
        }

        return result;
    }
</script>

@(Html.Kendo().DropDownListFor(m => m)
    .DataValueField("SubCategoryID")
    .DataTextField("SubCategoryName")
    .OptionLabel("請選擇")
    .DataSource(source => source
        .Read(read => read.Url("/api/Products/GetSubCategories").Data("filter")) // 傳遞額外參數至後端
        .ServerFiltering(true) // 啟用 Server Filtering
    )
    .AutoBind(false) // 關閉 Auto Binding
    .CascadeFrom(cascadeFrom)
    .Events(events =>
    {
        if (string.IsNullOrEmpty(onChange) == false)
        {
            events.Change(onChange);
        }
    })
    .HtmlAttributes(htmlAttributes)
)
```

#### 設定欄位的 EditorTemplate
* 使用 UIHint
* 使用 EditorTempalteName

##### 使用 UIHint
> ~\Models\Product.cs
```
[DapperTable("Products")]
public class Product
{
    ...
    
    [UIHint("Category")]
    [Display(Name = "類別")]
    public int? CategoryID { get; set; }
    
    ...
}
```

##### 使用 EditorTemplateName
> ~\Views\Grid\InlineEditing.cshtml
```
columns
    .Bound(p => p.SubCategory)
    .ClientTemplate("#=SubCategory?.SubCategoryName ?? ''#")
    .EditorTemplateName("SubCategory");
```

### Grid 展開 Detail（ClientDetailTemplateId）
> ~\Views\Grid\InlineEditing.cshtml
```
.ClientDetailTemplateId("spec-template")
```
* ToClientTemplate()
```
<script id="spec-template" type="text/kendo-tmpl">
    @(Html.Kendo().TabStrip()
    .Name("tabStrip_#=ProductID#")
    .SelectedIndex(0)
    .Animation(animation => animation.Open(open => open.Fade(FadeDirection.In)))
    .Items(items =>
    {
        items.Add().Text("Spec").Content(
            @<text>
                @(Html.Kendo().Grid<ProductSpec>()
                    .Name("grid_#=ProductID#")
                    .Columns(columns =>
                    {
                        columns.Bound(o => o.ProductSpecID).Title("ID").Width(100);
                        columns.Bound(o => o.SpecName).Width(200);
                        columns.Bound(o => o.Desctiption);
                    })
                    .DataSource(dataSource => dataSource
                        .Ajax()
                        .PageSize(5)
                        .Read(read => read.Action("Get", "ProductSpecs", new { productID = "#=ProductID#" }))
                    )
                    .Pageable()
                    .Sortable()
                    .ToClientTemplate())
            </text>
        );

        items.Add().Text("Category").Content(
            "<ul>" +
                "<li><label>UnitPrice:</label>#= UnitPrice #</li>" +
                "<li><label>UnitsInStock:</label>#= UnitsInStock #</li>" +
                "<li><label>UnitsOnOrder:</label>#= UnitsOnOrder #</li>" +
            "</ul>"
        );
    })
    .ToClientTemplate())
</script>
```

## Grid 事件
> ~\Views\Grid\InlineEditing.cshtml
```
.Events(events => events.BeforeEdit("onBeforeEdit").Save("onDataBound"))
```

```
<script type="text/javascript">
    function onBeforeEdit(e) {
        console.log('onBeforeEdit', e);
        alert('onBeforeEdit');
    }

    function onDataBound(e) {
        console.log('onDataBound', e);
        alert('onDataBound');
    }
</script>
```

## Grid DataSource 事件
> ~\Views\Grid\InlineEditing.cshtml
```
dataSource.Events(events => events.Error("error_handler"))
```

```
<script type="text/javascript">
    function error_handler(e) {
        if (e.errors) {
            var message = "Errors:\n";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function() {
                        message += this + "\n";
                    });
                }
            });
            alert(message);
        }
    }
</script>
```

## Grid 其他
### 從 Javascript 修改欄位值
```
var dataItem = grid.dataItem(tr);
dataItem.set('field_name', 'new_value');
```

### 增加資料至 DataSource
```
function add(){
    var grid = $("#grid").data("kendoGrid");
    grid.dataSource.add({ field_name: 'value' });
}
```

# DropdownList
```
@(Html.Kendo().DropDownList()
    .Name("category_1")
    .DataValueField("CategoryID")
    .DataTextField("CategoryName")
    .OptionLabel("請選擇")
    .BindTo(Category.Categories)
    .Value(2.ToString())
)
```

綁定 Model
```
@(Html.Kendo().DropDownListFor(m=> m.CategoryID)
    .DataValueField("CategoryID")
    .DataTextField("CategoryName")
    .OptionLabel("請選擇")
    .BindTo(Category.Categories)
)
```

# EditorTemplate
```
columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").EditorTemplateName("Date");
columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").EditorTemplateName("Time");
columns.Bound(p => p.LastSupply).Format("{0:yyyy/MM/dd}").EditorTemplateName("DateTime");
```

# TextBox Mask
> Demo：
> https://demos.telerik.com/aspnet-mvc/maskedtextbox

> 參數：
> https://docs.telerik.com/kendo-ui/api/javascript/ui/maskedtextbox/configuration/mask

# String Format
Grid Column 內容顯示樣式
```
columns.Bound(c => c.Field).Format("{0:n0}");
```

修改 Input 顯示樣式
```
@Html.Kendo().NumericTextBoxFor(m => m).Format(format)
```

> 其他樣式：
> https://docs.telerik.com/kendo-ui/globalization/intl/numberformatting

# Editor（Rich Text Editor）
因為安全性的問題，ASP.NET MVC 不允許傳送有「<」或「>」等 HTML Tag
若前端有編輯 HTML 的需求，請使用 Kendo UI 的 Editor
```
@(Html.Kendo().Editor(value)
@(Html.Kendo().EditorFor(m => m)
```
Editor 會自動編碼後再傳送至 Server 端
在後端接收到資料時，使用 HttpUtility.HtmlDecode 解碼，在儲存至資料庫
```
HttpUtility.HtmlDecode(value)
```
View 顯示時不需要解碼，而是使用 Html.Raw 顯示原始 HTML
```
@(Html.Raw(value))
```

# 檔案上傳
## 單一檔案上傳
表單內只有一個檔案上傳時，可在 Controller 使用擴充功能 GetFiles
```
using TabfMis.Extensions;

var attachments = Request.GetFiles();
```

## 多檔案上傳
### DTO
> TabfMis.Sample\Dtos\SampleItemDto.cs
```
public class FilesDto
{
    public virtual HttpPostedFileBase File1 { get; set; }
    public virtual HttpPostedFileBase File2 { get; set; }
}
```
### View
> Views\SampleItem\_Form.cshtml、Views\SampleItem\Edit.cshtml
```
@using (Html.BeginForm("Edit", "ControllerName", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    <div class="form-row">
        <div class="form-group col offset-2">
            @(Html.Kendo().Upload().Name(nameof(SampleItemDto.File1)))
        </div>
        <div class="form-group col">
            @(Html.Kendo().Upload().Name(nameof(SampleItemDto.File2)))
        </div>
    </div>
}
```

### Controller
> Controllers\SampleItemController.cs
```
using TabfMis.Extensions;

public ActionResult Edit(int id, FilesDto model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    if (model.File1 != null)
    {
        var fileName1 = Path.GetFileName(model.File1.FileName);
        var bytes1 = model.File1.GetBytes();
    }

    if (model.File2 != null)
    {
        var fileName2 = Path.GetFileName(model.File2.FileName);
        var bytes2 = model.File2.GetBytes();
    }

    return View(model);
}