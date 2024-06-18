using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Utility;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ABC.Client.Components.Pages.SalesInventory.SalesReport;

public partial class SalesReport
{
	#region Injections
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
	[Inject] ApplicationUserService_SQL applicationUserSevice_SQL { get; set; }
	[Inject] CustomerService_SQL customerService_SQL { get; set; }
	[Inject] IJSRuntime JSRuntime { get; set; }
	#endregion

	#region Fields
	private List<OrderHeader> orderHeadersList { get; set; } = [];
	private List<Product> productsList { get; set; } = [];

	//parameters
	private double totalSales;
	private double AddsomeSales;
	private double AheadSales;

	private double WalkInSales;
	private double OnCallSales;
	private double ChatBasedSales;
	private double ShoppingWebsiteSales;

	private double AddsomeWalkInSales;
	private double AddsomeOnCallSales;
	private double AddsomeChatBasedSales;

	private double AheadWalkInSales;
	private double AheadOnCallSales;
	private double AheadChatBasedSales;


	private double grossProfit;
	private double AddsomeProfit;
	private double AheadBizProfit;

	private List<(Product Product, int Count, float CostPrice, float RetailPrice)> Products { get; set; } = new List<(Product, int, float, float)>();
	private DateTime startDate = DateTime.Today.AddDays(-7);
	private DateTime endDate = DateTime.Today;
	private string errorMessage = "";
	#endregion

	protected override async Task OnInitializedAsync()
	{
		orderHeadersList = await orderHeaderService_SQL.GetFilterOrdersList(applicationDbContext);

		await LoadReport();
	}

	private async Task ApplyDate()
	{
		DateTime _startDate = startDate.Date;
		DateTime _endDate = endDate.Date;

		if (_endDate < _startDate)
		{
			errorMessage = "*End date cannot be earlier than start date.";
			await Task.Delay(2000);
			errorMessage = "";
		}
		else
		{
			errorMessage = "";
			orderHeadersList = await orderHeaderService_SQL.GetFilterOrdersList(applicationDbContext, string.Empty, _startDate, _endDate);
			await LoadReport();
		}
		await InvokeAsync(StateHasChanged);
	}

	private async Task LoadReport()
	{

		totalSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && (o.OrderDetails == null || o.OrderDetails.All(od => od.status != SD.StatusRefunded))).Sum(o => o.OrderTotal);
		var (addsomeSales, aheadSales) = await CalculateSalesPerStore();
		AddsomeSales = addsomeSales;
		AheadSales = aheadSales;

		totalSales += orderHeadersList
		.Where(o => o.OrderStatus == SD.StatusCompleted && (o.OrderDetails == null || o.OrderDetails.All(od => od.status != SD.StatusRefunded)))
		.Sum(o => o.DeliveryFee != null ? Convert.ToDouble(o.DeliveryFee) : 0);

		WalkInSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.WalkIn).Sum(o => o.OrderTotal);
		AddsomeWalkInSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.WalkIn && o.StoreName.Contains("Addsome")).Sum(o => o.OrderTotal);
		AheadWalkInSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.WalkIn && o.StoreName.Contains("Ahead")).Sum(o => o.OrderTotal);

		OnCallSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.OnCall).Sum(o => o.OrderTotal);
		AddsomeOnCallSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.OnCall && o.StoreName.Contains("Addsome")).Sum(o => o.OrderTotal);
		AheadOnCallSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.OnCall && o.StoreName.Contains("Ahead")).Sum(o => o.OrderTotal);

		ChatBasedSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.ChatBased).Sum(o => o.OrderTotal);
		AddsomeChatBasedSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.ChatBased && o.StoreName.Contains("Addsome")).Sum(o => o.OrderTotal);
		AheadChatBasedSales = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.ChatBased && o.StoreName.Contains("Ahead")).Sum(o => o.OrderTotal);

		ShoppingWebsiteSales = orderHeadersList
		.Where(o => o.OrderStatus == SD.StatusCompleted && o.SalesChannel == SD.ShopWeb)
		.Sum(o => o.OrderTotal + (o.DeliveryFee != null ? Convert.ToDouble(o.DeliveryFee) : 0));

		grossProfit = await CalculateGrossProfit();
		AddsomeProfit = await CalculateGrossProfitPerStore("Addsome Business Corporation");
		AheadBizProfit = await CalculateGrossProfitPerStore("Ahead Biz Computers");

		await InvokeAsync(StateHasChanged);
	}

	private async Task<(double AddsomeSales, double AheadSales)> CalculateSalesPerStore()
	{
		double addsomeSales = 0;
		double aheadSales = 0;

		var completedOrders = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted);

		foreach (var order in completedOrders)
		{
			if (order.OrderDetails == null || order.OrderDetails.Count == 0)
			{
				await applicationDbContext.Entry(order).Collection(o => o.OrderDetails).LoadAsync();
			}

			double deliveryFee = order.DeliveryFee != null ? Convert.ToDouble(order.DeliveryFee) : 0;

			foreach (var orderDetail in order.OrderDetails)
			{
				await applicationDbContext.Entry(orderDetail).Reference(od => od.Product).LoadAsync();

				if (order.StoreName.Contains("Addsome"))
				{
					addsomeSales += (orderDetail.Count * orderDetail.Product.RetailPrice);
				}
				else if (order.StoreName.Contains("Ahead Biz"))
				{
					aheadSales += (orderDetail.Count * orderDetail.Product.RetailPrice);
				}
			}

			// Adding delivery fee to sales
			if (order.StoreName.Contains("Addsome"))
			{
				addsomeSales += deliveryFee;
			}
			else if (order.StoreName.Contains("Ahead Biz"))
			{
				aheadSales += deliveryFee;
			}
		}

		return (addsomeSales, aheadSales);
	}

	private async Task<double> CalculateGrossProfit()
	{
		double profit = 0;

		var completedOrders = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted);

		foreach (var order in completedOrders)
		{
			if (order.OrderDetails == null || order.OrderDetails.Count == 0)
			{
				await applicationDbContext.Entry(order).Collection(o => o.OrderDetails).LoadAsync();
			}

			foreach (var orderDetail in order.OrderDetails)
			{
				await applicationDbContext.Entry(orderDetail).Reference(od => od.Product).LoadAsync();
			}

			profit += order.OrderDetails.Sum(od => (od.RetailPrice - od.CostPrice) * od.Count);
		}

		return profit;
	}

	private async Task<double> CalculateGrossProfitPerStore(string storeName)
	{
		double profit = 0;

		var completedOrders = orderHeadersList.Where(o => o.OrderStatus == SD.StatusCompleted && o.StoreName == storeName);

		foreach (var order in completedOrders)
		{
			if (order.OrderDetails == null || order.OrderDetails.Count == 0)
			{
				await applicationDbContext.Entry(order).Collection(o => o.OrderDetails).LoadAsync();
			}

			foreach (var orderDetail in order.OrderDetails)
			{
				await applicationDbContext.Entry(orderDetail).Reference(od => od.Product).LoadAsync();
			}

			profit += order.OrderDetails.Sum(od => (od.RetailPrice - od.CostPrice) * od.Count);
		}

		return profit;
	}

	public string FormatCurrency(double value)
	{
		return value.ToString("C", new CultureInfo("en-PH"));
	}

	private void Print()
	{
		JSRuntime.InvokeVoidAsync("Print");

	}
}