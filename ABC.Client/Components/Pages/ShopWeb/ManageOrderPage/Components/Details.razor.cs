using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using ABC.Client.Components.Account.Pages.Manage;
using System.Reflection.Emit;
using ABC.Shared.Utility;

namespace ABC.Client.Components.Pages.ShopWeb.ManageOrderPage.Components;

public partial class Details
{
    #region DEPENDENCY INJECTIOn
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }
    #endregion

    #region fields
    private List<OrderDetail> orderDetailsList { get; set; } = [];
    private OrderHeader orderHeader { get; set; } = new();
    private OrderDetail selectedOrder { get; set; } = new();
    private StockPerStore stockPerStore { get; set; } = new();

    [SupplyParameterFromQuery(Name = "id")]
    public int OrderId { get; set; }

    #endregion

    #region Variables
    private string? firstName;
    private string? phoneNumber;
    private string? lineAddress;
    private string? city;
    private string? province;
    private string? zipCode;
    private string? email;
    private double TotalToPay;

    #endregion
    protected override async Task OnInitializedAsync()
    {
        orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;

        orderDetailsList = await orderHeaderService_SQL.GetOrderDetailsList(applicationDbContext, OrderId);
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
        firstName = orderHeader.ApplicationUser?.FirstName;
        phoneNumber = orderHeader.ContactNumber;
        lineAddress = orderHeader.AddressLine;
        province = orderHeader.Province;
        zipCode = orderHeader.ZipCode;
        email = orderHeader.ApplicationUser?.Email;
        TotalToPay = orderHeader.OrderTotal + Convert.ToDouble(orderHeader.DeliveryFee);
    }

    private async Task SaveOrder()
    {

    }
    private async Task CancelOrder()
    {
        orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
        orderHeader.OrderStatus = SD.StatusCancelled;

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
}

