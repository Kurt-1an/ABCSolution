using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Serilog;
using Microsoft.AspNetCore.Components;
using ABC.Client.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ABC.Shared.Utility;
using ABC.Client.Components.Pages.ShopWeb.Cart.OrderCheckout;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ABC.Client.Components.Pages.StockTransfers;
public partial class StockTransferList
{
    #region DEPENDENCY INJECTIOn
    [Inject] POSService_SQL pOSService_SQL { get; set; }
    [Inject] ProductService_SQL ProductService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    [Inject] AuditService_SQL auditService_SQL { get; set; }
    #endregion

    #region FIELDS
    private ApplicationUser ApplicationUser { get; set; } = new();
    private List<Product> ProductList { get; set; } = [];
    private List<StockTransferAudit> stockAudit { get; set; } = [];
    private Product ProductInModal { get; set; } = new();
    private String ProductSearchInput { get; set; } = String.Empty;
    private bool ShowProductDropdown { get; set; } = false;
    private string? userId { get; set; } = "";
    private string UserEmail { get; set; } = String.Empty;
    private Toast toastRef { get; set; }
    private AddProductNotice Notice { get; set; } = new();
    private bool showNotice { get; set; } = new();

    #endregion

    protected override async Task OnInitializedAsync()
    {
        ProductInModal.StockPerStore ??= new();
        stockAudit = await auditService_SQL.GetStockAuditList(applicationDbContext);

        await GetUserBasicInfo();

		pOSService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
    }

    private async Task GetUserBasicInfo()
    {
		var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
		var claimsIdentity = user.Identity as ClaimsIdentity;
		userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		UserEmail = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
		ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);
	}

    private async Task GetProductList(ChangeEventArgs e)
    {
        ProductSearchInput = e?.Value?.ToString();
        ProductList.Clear();

        var result = await pOSService_SQL.GetProductList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(ProductSearchInput))
        {
            ProductList = result
                .Where(x => x.productName.ToString().StartsWith(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase) && x.StockPerStore.TotalStocks > 0).ToList();
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowProductDropdownHandler(bool show)
    {
        await Task.Delay(1000);
        ShowProductDropdown = show;
        StateHasChanged();
    }

    private async Task PopulateProductDetails(int productId)
    {
        ProductInModal = new();
        // Find the Item
        var result = await pOSService_SQL.GetProductInfo(applicationDbContext, productId);
        if (result is not null)
        {
            ProductInModal = result;
        }
    }

    private async Task<bool> TransferStocks(StockPerStore stockPerStore)
    {
        bool isSuccess = false;
        Notice = new();
        try
        {
            isSuccess = await ProductService_SQL.TransferStock(applicationDbContext, stockPerStore, UserEmail);
            Notice = isSuccess.BuildStockTransferNotice();
            showNotice = true;
            await InvokeAsync(StateHasChanged);

            await Task.Delay(2000).ContinueWith(_ =>
            {
                showNotice = false;
                InvokeAsync(StateHasChanged);
            });
            stockAudit = await auditService_SQL.GetStockAuditList(applicationDbContext);
            StateHasChanged();
            return isSuccess;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            Notice = isSuccess.BuildStockTransferNotice();
            showNotice = true;
            await InvokeAsync(StateHasChanged);

            await Task.Delay(3000).ContinueWith(_ =>
            {
                showNotice = false;
                InvokeAsync(StateHasChanged);
            });
            return isSuccess;
        }
    }
}