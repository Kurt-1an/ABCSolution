using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.OnlineCustomers;

public partial class OnlineCustomerDetails
{
    #region Injections
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    [Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    #endregion
    #region Fields
    private List<ApplicationUser> OnlineCustomers { get; set; } = [];
    private List<OrderHeader> Orders { get; set; } = [];
    private ApplicationUser SelectedCustomer { get; set; } = new();

    [SupplyParameterFromQuery(Name = "id")]
    public string onlineCustomerId { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        applicationUserService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadOnlineCustomer();
        await GetOrders(onlineCustomerId);
    }

    private async Task LoadOnlineCustomer()
    {
        var customerTask = applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, onlineCustomerId);
        SelectedCustomer = await customerTask;
    }

    private async Task GetOrders(string onlineCustId)
    {
        OnlineCustomers = await applicationUserService_SQL.GetApplicationUserList(applicationDbContext);
        Orders = await orderHeaderService_SQL.GetOrderHeaderByUserId(applicationDbContext, onlineCustomerId);
    }
}
