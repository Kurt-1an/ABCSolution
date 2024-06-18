using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Reflection.Emit;
using ABC.Shared.Utility;
using Serilog;
using System.Security.Claims;
using System.Globalization;
using Microsoft.JSInterop;
using MySqlX.XDevAPI.Common;

namespace ABC.Client.Components.Pages.SalesInventory.OrderPage.details;
public partial class Details
{
	#region DEPENDENCY INJECTIOn
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] StoreService_SQL storeService_SQL { get; set; }

	[Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
	[Inject] IJSRuntime JSRuntime { get; set; }

    #endregion

    #region fields
    private ApplicationUser ApplicationUser { get; set; } = new();
	private Store Store { get; set; } = new();

	[CascadingParameter]
	private HttpContext HttpContext { get; set; } = default!;


	private Toast toastRef;
	private List<OrderDetail> orderDetailsList { get; set; } = [];
	private OrderHeader orderHeader { get; set; } = new();
	private OrderDetail selectedOrder { get; set; } = new();


	[SupplyParameterFromQuery(Name = "id")]
	public int OrderId { get; set; }
    private string? userId { get; set; } = "";
	private StockPerStore stockPerStore { get; set; } = new();
	private bool ShowStockTransferAlert { get; set; } = false;

	#endregion

	#region Variables
	private string? firstName;
	private string? lastName;
	private string? phoneNumber;
	private string? lineAddress;
	private string? city;
	private string? province;
	private string? zipCode;
	private string? email;
	private decimal charges;
	private double TotalToPay;
	bool showOtherReason = false;
	string otherReason = "";
	private bool reason1Selected;
	private bool reason2Selected;
	private bool reason3Selected;
	private string prodName;
	private string? ErrorMessage;
	private int storeQty;
	#endregion


	protected override async Task OnInitializedAsync()
	{
		orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		orderDetailsList = await orderHeaderService_SQL.GetOrderDetailsList(applicationDbContext, OrderId);
        await LoadProducts();
		await LoadUser();

	}

	private async Task LoadUser()
	{
		// GET USER
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = authState.User;
		var claimsIdentity = user.Identity as ClaimsIdentity;
		userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		ApplicationUser = await applicationUserService_SQL.GetCurrentUserInfo(applicationDbContext, userId);

		if (ApplicationUser != null)
		{
			if (ApplicationUser.StoreId != null)
			{
				Store = await storeService_SQL.GetStoreInfo(applicationDbContext, ApplicationUser.StoreId.Value);
			}
		}
	}


	private async Task LoadProducts()
	{
		orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);

		if (orderHeader.ApplicationUser != null)
		{
			firstName = orderHeader.ApplicationUser.FirstName;
			lastName = orderHeader.ApplicationUser.LastName;
            phoneNumber = orderHeader.ContactNumber;
			lineAddress = orderHeader.AddressLine;
            city = orderHeader.City;
			province = orderHeader.Province;
			zipCode = orderHeader.ZipCode;
			email = orderHeader.ApplicationUser.Email;
		}
		else if (orderHeader.Customer != null)
		{
			firstName = orderHeader.Customer.FirstName;
			lastName = orderHeader.Customer.LastName;
            phoneNumber = orderHeader.ContactNumber;
            lineAddress = orderHeader.AddressLine;
            city = orderHeader.City;
            province = orderHeader.Province;
            zipCode = orderHeader.ZipCode;
            email = orderHeader.Customer.EmailAddress;
		}

		charges = orderHeader.DeliveryFee + orderHeader.ServiceFee;
		TotalToPay = orderHeader.OrderTotal + Convert.ToDouble(orderHeader.DeliveryFee);
	}

	private async Task SaveOrder()
	{

	}

	private async Task StartProcessing()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = authState.User;

		orderHeader.OrderStatus = SD.StatusProcessing;
		orderHeader.ProcessDate = DateTime.UtcNow;
		orderHeader.ProcessedBy = user.Identity.Name;

		// Call service to update OrderHeader
		bool updated = await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		if (updated)
		{
			//refresh the list
			StateHasChanged();
		}
	}

	private async Task DeliverOrder()
	{
		if (string.IsNullOrWhiteSpace(orderHeader.Carrier))
		{
			toastRef.ShowToast("Note", "Please enter a delivery personnel");
			return;
		}

		orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
		orderHeader.Carrier = orderHeader.Carrier;
		orderHeader.OrderStatus = SD.StatusShipped;
		orderHeader.ShippingDate = DateTime.UtcNow;

		// Call service to update OrderHeader
		bool updated = await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		if (updated)
		{
			//refresh the list
			StateHasChanged();
		}
	}
	private async Task OrderPaid()
	{
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
		orderHeader.OrderStatus = SD.StatusCompleted;
		orderHeader.PaymentStatus = SD.StatusCompleted;
		orderHeader.CompletionDate = DateTime.UtcNow;
		orderHeader.CompletedBy = user.Identity.Name;

        // Call service to update OrderHeader
        bool updated = await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		if (updated)
		{
			//refresh the list
			StateHasChanged();
		}
	}

	private async Task CancelOrder()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = authState.User;

		orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
		orderHeader.OrderStatus = SD.StatusCancelled;
		orderHeader.CancellationDate = DateTime.UtcNow;
		orderHeader.CancelledBy = user.Identity.Name;

		List<StockPerStore> stockList = await productService_SQL.GetStockPerStoreList(applicationDbContext);

		foreach (var orderdetails in orderHeader.OrderDetails)
		{
			var stock = stockList.FirstOrDefault(x => x.ProductId == orderdetails.ProductId);
			stock.Store1StockQty += orderdetails.Count;
			stock.TotalStocks = stock.Store1StockQty + stock.Store2StockQty;
			stockList.Remove(new StockPerStore()
			{
				ProductId = orderdetails.ProductId,
			});
			stockList.Add(stock);
		}

		// Call service to update OrderHeader
		bool updated = await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		if (updated)
		{
			await productService_SQL.UpdateStockPerStore(applicationDbContext, stockPerStore, string.Empty);
			StateHasChanged();
		}
	}


	private async Task RefundOrder(int Id)
	{
		//GET USER
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = authState.User;

		//UPDATE STATUSES OF ORDER HEADER
		orderHeader.OrderStatus = SD.StatusRefunded;
		orderHeader.RefundedBy = user.Identity.Name;

		await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		//GET ORDER DETAIL
		selectedOrder = await orderHeaderService_SQL.GetOrderDetail(applicationDbContext, Id);



		if (selectedOrder != null && selectedOrder.Product != null && selectedOrder.Product.StockPerStore != null)
		{
			if (orderHeader.StoreName.Contains("Addsome"))
			{
				selectedOrder.Product.StockPerStore.Store1StockQty -= selectedOrder.remarkQty ?? 0;
				selectedOrder.Product.StockPerStore.TotalStocks -= selectedOrder.remarkQty ?? 0;
				await productService_SQL.UpdateStockPerStore(applicationDbContext, selectedOrder.Product.StockPerStore, orderHeader.RefundedBy);

			}
			else if (orderHeader.StoreName.Contains("Ahead"))
			{
				selectedOrder.Product.StockPerStore.Store2StockQty -= selectedOrder.remarkQty ?? 0;
				selectedOrder.Product.StockPerStore.TotalStocks -= selectedOrder.remarkQty ?? 0;
				await productService_SQL.UpdateStockPerStore(applicationDbContext, selectedOrder.Product.StockPerStore, orderHeader.RefundedBy);
			}
		}



		//UPDATE ORDER DETAIL STATUS
		AddReasons();
		selectedOrder.status = SD.ProductRefunded;
		bool updated = await orderHeaderService_SQL.UpdateOrderDetailStatus(applicationDbContext, selectedOrder);

		if (updated)
		{
			ResetReasons();
			StateHasChanged();
		}
	}

	public string FormatCurrency(double? value)
	{
		if (value.HasValue)
		{
			return value.Value.ToString("C", new CultureInfo("en-PH"));
		}
		else
		{
			return string.Empty; 
		}
	}

	private void Print()
	{
		JSRuntime.InvokeVoidAsync("Print");

	}

	#region REFUND METHODS
	private async Task PopulateOrder(int Id)
	{

		selectedOrder = new();

		// Find the Item
		var result = await orderHeaderService_SQL.GetOrderDetail(applicationDbContext, Id);
		var prodId = result.ProductId;
		var storeStockStore = await productService_SQL.GetStockperStoreInfo(applicationDbContext, prodId);

		if (result is not null)
		{
			selectedOrder = result;
			prodName = selectedOrder.Product.productName;

			if (orderHeader.StoreName.Contains("Addsome"))
			{
				storeQty = storeStockStore.Store1StockQty;
			}
			else if (orderHeader.StoreName.Contains("Ahead"))
			{
				storeQty = storeStockStore.Store2StockQty;
			}
		}
		await InvokeAsync(StateHasChanged);
	}


	private void AddReasons()
	{
		// Build the Remarks based on selected reasons
		List<string> selectedReasons = new List<string>();
		if (reason1Selected)
		{
			selectedReasons.Add("Item not as described");
		}
		if (reason2Selected)
		{
			selectedReasons.Add("Item damaged during shipment");
		}
		if (reason3Selected)
		{
			selectedReasons.Add("Wrong item received");
		}
		if (!string.IsNullOrEmpty(otherReason))
		{
			selectedReasons.Add(otherReason);
		}

		selectedOrder.Remark = string.Join(", ", selectedReasons);
	}


	private void ResetReasons()
	{
		reason1Selected = false;
		reason2Selected = false;
		reason3Selected = false;
		otherReason = "";
	}

	void ShowOtherReason()
	{
		showOtherReason = !showOtherReason;
	}
	#endregion
}

