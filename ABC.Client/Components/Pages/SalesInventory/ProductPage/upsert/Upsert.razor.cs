using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using System.Security.Claims;


namespace ABC.Client.Components.Pages.SalesInventory.ProductPage.upsert;
    public partial class Upsert
    {

    #region DEPENDENCY INJECTIOn
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] CategoryService_SQL categoryService_SQL { get; set; }
    [Inject] SupplierService_SQL supplierService_SQL { get; set; }
    [Inject] StoreService_SQL storeService_SQL { get; set; }
    [Inject] IWebHostEnvironment _webHostEnvironment { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }

	#endregion

	#region fields
	private List<Product> Products { get; set; } = [];
    private List<Category> CategoryList { get; set; } = [];
    private List<Supplier> SupplierList { get; set; } = [];
    private List<Store> StoreList { get; set; } = [];
    private AddProductNotice Notice { get; set; } = new();
    private bool showNotice { get; set; } = false;
    private Product SelectedProduct { get; set; } 
    private StockPerStore StockPerStoreInput { get; set; } = new();
    private IBrowserFile selectedFile;
    private string previewImageData = null;

    [SupplyParameterFromQuery(Name = "id")]
	public int ProductId { get; set; }
    public int minimumStock = 1;

	private ApplicationUser ApplicationUser { get; set; } = new();
	#endregion

	protected override async Task OnInitializedAsync()
    {
        productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await GetUserBasicInfo();
		await LoadProducts();
	}

	private async Task GetUserBasicInfo()
	{
		var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
		var claimsIdentity = user.Identity as ClaimsIdentity;
		string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);
	}

	private async Task LoadProducts()
    {
        // Load product information, category list, store list, and supplier list concurrently
        var productTask = productService_SQL.GetProductInfo(applicationDbContext, ProductId);
        var categoryTask = categoryService_SQL.GetCategoryListData(applicationDbContext);
        var storeTask = storeService_SQL.GetStoreListData(applicationDbContext);
        var supplierTask = supplierService_SQL.GetSupplierList(applicationDbContext);

        // Await all tasks simultaneously
        SelectedProduct = await productTask;
        CategoryList = await categoryTask;
        StoreList = await storeTask;
        SupplierList = await supplierTask;

    }

    private async Task SaveProduct()
    {
        if (SelectedProduct.Id == 0)
        {
            SelectedProduct.StartSalesDate = DateTime.UtcNow;
            // If the Id is 0, it's a new product
            if (selectedFile != null)
            {
                // Handle file upload
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(selectedFile.Name);
                string productPath = Path.Combine(wwwRootPath, @"image\product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    await selectedFile.OpenReadStream().CopyToAsync(fileStream);
                }

                SelectedProduct.ImageUrl = $@"\image\product\{fileName}";
            }

            StockPerStoreInput.TotalStocks = StockPerStoreInput.Store1StockQty + StockPerStoreInput.Store2StockQty;

            // NOTIFICATION MODAL
            bool added = await productService_SQL.AddProduct(applicationDbContext, SelectedProduct, StockPerStoreInput, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");
            if(added){
                Notice = added.BuildAddProductNotice(); 
                showNotice = true;

                // RESETS
                StockPerStoreInput = new();
                SelectedProduct = new();

                await Task.Delay(3000).ContinueWith( _ => {
                    showNotice = false;
                    NavigationManager.NavigateTo("/ProductList", true);
                });
            }else{
                Notice = added.BuildAddProductNotice();
                showNotice = true;
                await Task.Delay(3000).ContinueWith( _ => {
                    showNotice = false;
                    InvokeAsync(StateHasChanged);
                });
            }

        }
        else
        {
            // If the Id is not 0, it's an existing product to be updated
            if (selectedFile != null)
            {
                // Handle file upload and deletion of old image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(selectedFile.Name);
                string productPath = Path.Combine(wwwRootPath, @"image\product");

                if (!string.IsNullOrEmpty(SelectedProduct.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, SelectedProduct.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    await selectedFile.OpenReadStream().CopyToAsync(fileStream);
                }

                SelectedProduct.ImageUrl = $@"\image\product\{fileName}";
            }

            bool updated = await productService_SQL.UpdateProduct(applicationDbContext, SelectedProduct, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");
            bool updatedStock = await productService_SQL.UpdateStockPerStore(applicationDbContext, SelectedProduct.StockPerStore, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");
            if (updated || updatedStock)
            {
                NavigationManager.NavigateTo("/ProductList", true);
            }
        }

        if (StockPerStoreInput.Store1StockQty <= StockPerStoreInput.MinimumStore1StockQty && StockPerStoreInput.Store1StockQty > 0)
        {
            SelectedProduct.status = SD.AddsomeLowStock;
        }
        else if (StockPerStoreInput.Store2StockQty <= StockPerStoreInput.MinimumStore2StockQty && StockPerStoreInput.Store2StockQty > 0)
        {
            SelectedProduct.status = SD.AheadLowStock;
        }
        else if (StockPerStoreInput.Store1StockQty == 0)
        {
            SelectedProduct.status = SD.AddsomeOutOfStock;
        }
        else if (StockPerStoreInput.Store2StockQty == 0)
        {
            SelectedProduct.status = SD.AheadOutOfStock;
        }
        else if ((StockPerStoreInput.TotalStocks > StockPerStoreInput.MinimumStore1StockQty) && (StockPerStoreInput.TotalStocks > StockPerStoreInput.MinimumStore2StockQty))
        {
            SelectedProduct.status = SD.InStock;
        }

    }

    private async Task CancelAction()
	{
		if (SelectedProduct.Id != 0)
		{
			SelectedProduct = await productService_SQL.GetProductInfo(applicationDbContext, SelectedProduct.Id);
			NavigationManager.NavigateTo("/ProductList", true);
		}
		else
		{
			SelectedProduct = new Product();
			NavigationManager.NavigateTo("/ProductList", true);
		}
	}

	private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;

        // Read the file and convert it to a base64-encoded string
        var memoryStream = new MemoryStream();
        await e.File.OpenReadStream().CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        previewImageData = $"data:{e.File.ContentType};base64,{Convert.ToBase64String(fileBytes)}";

        StateHasChanged();
    }

}

