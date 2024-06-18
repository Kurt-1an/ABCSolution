using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using MySqlX.XDevAPI.Common;

namespace ABC.Client.Components.Pages.ShopWeb.Home;

public partial class Shop
{
	#region Injections
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] ProductService_SQL productService_SQL { get; set; }
	#endregion

	#region fields
	private List<Product> ProductList { get; set; } = [];

	private String ProductSearchInput { get; set; } = String.Empty;
	#endregion

	#region
	protected override async Task OnInitializedAsync()
	{
		productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		ProductList = await productService_SQL.GetProductList(applicationDbContext);
	}
	private async Task GetProductList(ChangeEventArgs e)
	{
		ProductSearchInput = e?.Value?.ToString();

		var result = await productService_SQL.GetProductList(applicationDbContext);
		if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(ProductSearchInput))
		{
			ProductList = result.Where(x => x.productName.ToString().Contains(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
		else
		{
			ProductList = result.ToList();
		}
		await InvokeAsync(StateHasChanged);
	}

	#endregion

}
