using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Implementation;
using Microsoft.JSInterop;
using System.Security.Claims;
using ABC.Shared.Utility;
using System.Globalization;

namespace ABC.Client.Components.Pages.SalesInventory.Dashboard;
public partial class Dashboard
{
	#region INJECTIONS
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
	[Inject] ProductService_SQL productService_SQL { get; set; }
	[Inject] CustomerService_SQL customerService_SQL { get; set; }
	[Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    #endregion

    #region FIELDS
    // Lists
    private List<Customer> customersList { get; set; } = new List<Customer>();
	private List<ApplicationUser> onlinecustomersList { get; set; } = new List<ApplicationUser>();
	private List<OrderHeader> orderHeadersList { get; set; } = new List<OrderHeader>();
	private List<Product> productsList { get; set; } = new List<Product>();
	private List<Product> BestSellingProducts { get; set; } = new();
	private List<(Product Product, int Count)> Products { get; set; } = new List<(Product, int)>();
	// Parameters
	private int totalcustomers;
	private int unprocessedOrders;
	private int ordersoutfordelivery;
	private int cancelledOrders;

	private int stockAddsome;
	private int stockAhead;
	private int lowstockAddsome;
	private int lowstockAhead;
	private int outofstockAddsome;
	private int outofstockAhead;

	private double AddsomeSales;
	private double AheadBizSales;
	#endregion

	protected override async Task OnInitializedAsync()
	{
		productsList = await productService_SQL.GetProductList(applicationDbContext);
		orderHeadersList = await orderHeaderService_SQL.GetOrdersList(applicationDbContext);
		customersList = await customerService_SQL.GetCustomersList(applicationDbContext);
		onlinecustomersList = await applicationUserService_SQL.GetApplicationUserList(applicationDbContext);

		unprocessedOrders = orderHeadersList.Count(o => o.OrderStatus == SD.StatusPending);
		ordersoutfordelivery = orderHeadersList.Count(o => o.OrderStatus == SD.StatusShipped);
		cancelledOrders = orderHeadersList.Count(o => o.OrderStatus == SD.StatusCancelled);

		stockAddsome = productsList.Sum(p => p.StockPerStore.Store1StockQty);
		stockAhead = productsList.Sum(p => p.StockPerStore.Store2StockQty);

		lowstockAddsome = productsList.Count(p => p.status == SD.AddsomeLowStock || p.StockPerStore.Store1StockQty > 0 && p.StockPerStore.Store1StockQty <= p.StockPerStore.MinimumStore1StockQty);
		lowstockAhead = productsList.Count(p => p.status == SD.AheadLowStock || p.StockPerStore.Store2StockQty > 0 && p.StockPerStore.Store2StockQty <= p.StockPerStore.MinimumStore2StockQty);

		outofstockAddsome = productsList.Count(p => p.status == SD.AddsomeOutOfStock || p.StockPerStore.Store1StockQty == 0);
        outofstockAhead = productsList.Count(p => p.status == SD.AheadOutOfStock || p.StockPerStore.Store2StockQty == 0);

        var allcustomers = customersList.Count() + onlinecustomersList.Where(c => c.Role == SD.Role_Customer).Count();
        totalcustomers = allcustomers;

        var (addsomeSales, aheadSales) = await CalculateSalesPerStore();
        AddsomeSales = addsomeSales;
        AheadBizSales = aheadSales;

        await LoadBestSelling();
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

            foreach (var orderDetail in order.OrderDetails)
            {
                await applicationDbContext.Entry(orderDetail).Reference(od => od.Product).LoadAsync();

                if (order.StoreName.Contains("Addsome"))
                {
                    addsomeSales += orderDetail.Count * orderDetail.Product.RetailPrice;
                }
                else if (order.StoreName.Contains("Ahead Biz"))
                {
                    aheadSales += orderDetail.Count * orderDetail.Product.RetailPrice;
                }
            }
        }

        return (addsomeSales, aheadSales);
    }


    private async Task LoadBestSelling()
	{
		var productGroups = await applicationDbContext.OrderDetails
					.Include(detail => detail.Product)
					.GroupBy(detail => detail.Product)
					.Select(group => new
					{
						Product = group.Key,
						Count = group.Sum(detail => detail.Count)
					})
					.OrderByDescending(group => group.Count)
					.ToListAsync();

		Products = productGroups.Select(group => (group.Product, group.Count)).ToList();

		await InvokeAsync(StateHasChanged);
	}

	public string FormatCurrency(double value)
	{
		return value.ToString("C", new CultureInfo("en-PH"));
	}
}