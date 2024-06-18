using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Globalization;
using System.Text;

namespace ABC.Client.Components.Pages.SalesInventory.InventoryReport;

public partial class InventoryReport
{
    #region Injections
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] PurchaseOrderService_SQL purchaseOrderService_SQL { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] PdfService pdfService { get; set; }
    #endregion

    #region FIELDS
    private List<Product> productsList { get; set; } = new List<Product>();
    private List<PurchaseOrder> purchaseOrders { get; set; } = new List<PurchaseOrder>();
    private List<(Product Product, int TotalStock)> Products { get; set; } = new List<(Product, int)>();

    private double totalInventoryValue;
    private double AddsomeInventoryValue;
    private double AheadBizInventoryValue;

    private int totalAvailableQuantity;
    private int AddsomeAvailableQuantity;
    private int AheadAvailableQuantity;

    private DateTime startDate = DateTime.Today.AddDays(-7);
    private DateTime endDate = DateTime.Today;
    private string errorMessage = "";
    #endregion

    protected override async Task OnInitializedAsync()
    {
        productsList = await productService_SQL.GetProductList(applicationDbContext);
        purchaseOrders = await purchaseOrderService_SQL.GetPurchaseOrderList(applicationDbContext);

        await LoadInventoryReport();
        
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
            productsList = await productService_SQL.GetProductList(applicationDbContext);
            purchaseOrders = await purchaseOrderService_SQL.GetPurchaseOrderList(applicationDbContext);

            productsList = productsList.Where(p =>
                (!p.IsRemoved.HasValue || !p.IsRemoved.Value || (p.IsRemoved.Value && (_startDate <= p.EndSalesDate)))
            ).ToList();
            purchaseOrders = purchaseOrders.Where(po => po.Timestamp >= _startDate && po.Timestamp <= _endDate).ToList();

            await LoadInventoryReport();
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadInventoryReport()
    {
        productsList = productsList.Where(p =>
                (!p.IsRemoved.HasValue || !p.IsRemoved.Value || (p.IsRemoved.Value && (startDate <= p.EndSalesDate)))
            ).ToList();
        totalAvailableQuantity = productsList.Where(p => p.status == SD.InStock || p.status == SD.LowStock).Sum(p => p.StockPerStore.TotalStocks);
        AddsomeAvailableQuantity = productsList.Where(p => p.status == SD.InStock || p.status == SD.LowStock).Sum(p => p.StockPerStore.Store1StockQty);
        AheadAvailableQuantity = productsList.Where(p => p.status == SD.InStock || p.status == SD.LowStock).Sum(p => p.StockPerStore.Store2StockQty);

        totalInventoryValue = productsList.Sum(p => p.CostPrice * p.StockPerStore.TotalStocks);
        AddsomeInventoryValue = productsList.Sum(p => p.CostPrice * p.StockPerStore.Store1StockQty);
        AheadBizInventoryValue = productsList.Sum(p => p.CostPrice * p.StockPerStore.Store2StockQty);
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