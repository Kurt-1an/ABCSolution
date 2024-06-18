using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ABC.Shared.Utility;
using MySqlX.XDevAPI.Common;


namespace ABC.Client.Components.Pages.SalesInventory.ProductPage;

public partial class RefundedProd
{
	#region DEPENDENCY INJECTIOn
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

	#endregion

	#region fields
	private Toast toastRef;
	private List<OrderDetail> orderDetailsList { get; set; } = [];
	private OrderHeader orderHeader { get; set; } = new();
	private OrderDetail selectedOrder { get; set; } = new();
	private Product selectedProduct { get; set; } = new();
	private StockPerStore stockPerStore { get; set; } = new();
	private ApplicationUser ApplicationUser { get; set; } = new();
	private String SearchInput { get; set; } = String.Empty;
    private string? prodName;
	private int restocked;


	private int maxQty = 0;
	private string storeName = string.Empty;
	private int? mergeQty;
	#endregion

	protected override async Task OnInitializedAsync()
    {
		orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;

		var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
		var claimsIdentity = user.Identity as ClaimsIdentity;
		string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		ApplicationUser = await applicationUserService_SQL.GetCurrentUserInfo(applicationDbContext, userId);

		await LoadProducts();
	}

    private async Task LoadProducts()
    {
		orderDetailsList = await orderHeaderService_SQL.GetOrderDetailList(applicationDbContext);
	}

    private async Task SearchFeature(ChangeEventArgs e)
    {
        SearchInput = e?.Value?.ToString();

        var result = await orderHeaderService_SQL.GetOrderDetailList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SearchInput))
        {
            orderDetailsList = result.Where(x => x.Product.productName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
			x.Remark!.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        else
        {
            orderDetailsList = result.ToList();
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task PopulateOrder(int Id)
	{
		selectedOrder = new();
		selectedProduct = new();

		// GET ORDER DETAIL
		var result = await orderHeaderService_SQL.GetOrderDetail(applicationDbContext, Id);

		//GET STOCK PER STORE
		var prodId = result.ProductId;
		stockPerStore = await productService_SQL.GetStockperStoreInfo(applicationDbContext, prodId);

		if (result is not null)
		{
			selectedOrder = result;
			prodName = selectedOrder.Product.productName;
			mergeQty = selectedOrder.remarkQty;

			if (ApplicationUser != null)
			{
				if (ApplicationUser.StoreName!.Contains("Addsome"))
				{
					maxQty = stockPerStore.Store1StockQty;
					storeName = "Addsome";
				}
				else if (ApplicationUser.StoreName!.Contains("Ahead"))
				{
					maxQty = stockPerStore.Store2StockQty;
					storeName = "Ahead";
				}
			}
		}
		await InvokeAsync(StateHasChanged);
	}

	private async Task Merge()
	{
		if (ApplicationUser != null)
		{
			if (ApplicationUser.StoreName!.Contains("Addsome"))
			{
				stockPerStore.Store1StockQty += selectedOrder.remarkQty ?? 0;
			}
			else if (ApplicationUser.StoreName!.Contains("Ahead"))
			{
				stockPerStore.Store2StockQty += selectedOrder.remarkQty ?? 0;
			}

			//UPDATES STOCK PER STORE (MERGES/ADDS THE RETURNED PRODUCTS BACK TO PRODUCT LIST)
			stockPerStore.TotalStocks += selectedOrder.remarkQty ?? 0;
			await productService_SQL.UpdateStockPerStore(applicationDbContext, stockPerStore, string.Empty);

		}

		//UPDATES ORDER DETAIL STATUS TO 'PRODUCT MERGED'
		selectedOrder.status = SD.RefundedProduct;
		await orderHeaderService_SQL.UpdateOrderDetailStatus(applicationDbContext, selectedOrder);
	}				
}

