using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ABC.Shared.Utility;
using System.Diagnostics;



namespace ABC.Client.Components.Pages.SalesInventory.ProductPage;

public partial class ProductList
{
	#region DEPENDENCY INJECTIOn
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
	[Inject] AuditService_SQL auditService_SQL { get; set; }
	[Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
	#endregion

	#region fields
	private string activeProduct = "all";
	private string all { get; set; } = "text-primary";
	private string addsome { get; set; } = "text-primary";
	private string ahead { get; set; } = "text-primary";
	private string Inactive { get; set; } = "text-danger";

	[SupplyParameterFromQuery(Name = "store")]
	public string Store { get; set; } = String.Empty;

	private List<Product> Products { get; set; } = [];
	private Product selectedProduct { get; set; } = new();
	private String ProductSearchInput { get; set; } = String.Empty;
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

	private async Task UpdateProductStatus(Product product)
    {
		if (product.IsRemoved == true) {
			product.status = SD.Archived;
		} 
		else if (product.StockPerStore.TotalStocks == 0)
		{
			product.status = SD.OutOfStock;
		}
		else if (product.StockPerStore.Store1StockQty <= product.StockPerStore.MinimumStore1StockQty &&
				 product.StockPerStore.Store2StockQty <= product.StockPerStore.MinimumStore2StockQty)
		{
			product.status = SD.LowStock;
		}
		else if (product.StockPerStore.Store1StockQty <= product.StockPerStore.MinimumStore1StockQty)
		{
			product.status = SD.AddsomeLowStock;
		}
		else if (product.StockPerStore.Store2StockQty <= product.StockPerStore.MinimumStore2StockQty)
		{
			product.status = SD.AheadLowStock;
		}
		else
		{
			product.status = SD.InStock;
		}


        // Update the product status in the database
        bool updated = await productService_SQL.UpdateProduct(applicationDbContext, product, string.Empty);

    }

    private async Task LoadProducts()
	{
		Products = await productService_SQL.GetProductList(applicationDbContext);
        foreach (var product in Products)
        {
			await UpdateProductStatus(product);
        }
	}


	private async Task GetProductList(ChangeEventArgs e)
	{
		ProductSearchInput = e?.Value?.ToString();

		var result = await productService_SQL.GetProductList(applicationDbContext);
		if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(ProductSearchInput))
		{
			Products = result.Where(x => x.productName.ToString().Contains(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
		else
		{
			Products = result.ToList();
		}
		StateHasChanged();
	}

	private async Task PopulateProductDetails(int productId)
	{
		selectedProduct = new();
		var result = await productService_SQL.GetProductInfo(applicationDbContext, productId);
		if (result is not null)
		{
			selectedProduct = result;
		}
		await InvokeAsync(StateHasChanged);
	}


	private async Task Remove()
	{
		selectedProduct.IsRemoved = true;
		selectedProduct.status = SD.Archived;
        selectedProduct.EndSalesDate = DateTime.UtcNow;

        bool removed = await productService_SQL.RemoveProduct(applicationDbContext, selectedProduct, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");

		if (removed)
		{
			//refresh the list
			await LoadProducts();
		}
	}


}

