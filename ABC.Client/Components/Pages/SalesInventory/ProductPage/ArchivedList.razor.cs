using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ABC.Shared.Utility;
using System.Diagnostics;

namespace ABC.Client.Components.Pages.SalesInventory.ProductPage;

	public partial class ArchivedList
	{
	#region DEPENDENCY INJECTIOn
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
	[Inject] AuditService_SQL auditService_SQL { get; set; }
	[Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }

	#endregion

	#region fields
	private string activeStore = "all"; // Track the active status
	private string all { get; set; } = "text-primary";
	private string addsome { get; set; } = "text-primary";
	private string ahead { get; set; } = "text-primary";

	private List<Product> Products { get; set; } = [];
	private Product selectedProduct { get; set; } = new();
	private String ProductSearchInput { get; set; } = String.Empty;
	private ApplicationUser ApplicationUser { get; set; } = new();


	#endregion

	protected override async Task OnInitializedAsync()
	{
		productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		await LoadProducts();
		await GetUserBasicInfo();
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
		Products = await productService_SQL.GetProductList(applicationDbContext);
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
		await InvokeAsync(StateHasChanged);
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


	private async Task Merge()
	{
		selectedProduct.IsRemoved = false;
		selectedProduct.status = SD.InStock;

		// Call service to remove product 
		await productService_SQL.UpdateProduct(applicationDbContext, selectedProduct, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");

	}
}

