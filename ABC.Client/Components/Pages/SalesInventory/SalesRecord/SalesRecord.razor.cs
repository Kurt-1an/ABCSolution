using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using System.Security.Claims;

namespace ABC.Client.Components.Pages.SalesInventory.SalesRecord;

public partial class SalesRecord
{
    #region DEPENDENCY INJECTIOn
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }

    [Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject] IHttpContextAccessor HttpContextAccessor { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    #endregion

    #region FIELDS
    private string activeStatus = "all"; // Track the active status
    private string all { get; set; } = "text-primary";
    private string cancelled { get; set; } = "text-danger";
    private string completed { get; set; } = "text-success";
    private string refunded { get; set; } = "text-secondary";


    public HttpContext? HttpContext { get; set; }
    private string userId;
    private List<OrderHeader> OrderHeader { get; set; } = new List<OrderHeader>();
    public ApplicationUser User { get; set; }
    private String SearchInput { get; set; } = String.Empty;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadProducts();
        SetActiveStatus(activeStatus);
    }

    private async Task LoadProducts()
    {
        OrderHeader = await orderHeaderService_SQL.GetOrdersList(applicationDbContext);
        FilterOrdersByStatus(activeStatus);
    }

    private async Task SearchFeature(ChangeEventArgs e)
    {
        SearchInput = e?.Value?.ToString();

        var result = await orderHeaderService_SQL.GetOrdersList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SearchInput))
        {
            OrderHeader = result.Where(x =>
                (x.Id.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.StoreName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.SalesChannel.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.PaymentMode.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                (x.Customer?.FirstName?.Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ?? false) ||
                (x.Customer?.LastName?.Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ?? false) ||
                (x.ApplicationUser?.FirstName?.Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ?? false) ||
                (x.ApplicationUser?.LastName?.Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ?? false))
                ).ToList();
        }
        else
        {
            OrderHeader = result?.ToList() ?? new List<OrderHeader>();
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task SetActiveStatus(string status)
    {
        activeStatus = status;
        all = "text-primary";
        cancelled = "text-primary";
        completed = "text-primary";
        refunded = "text-primary";

        switch (status)
        {
            case "cancelled":
                cancelled = "active text-white bg-danger";
                break;
            case "completed":
                completed = "active text-white bg-success";
                break;
            case "refunded":
                refunded = "active text-white bg-secondary";
                break;
            default:
                all = "active text-white bg-primary";
                break;
        }

        FilterOrdersByStatus(status);
    }

    private async Task FilterOrdersByStatus(string status)
    {
        OrderHeader = await orderHeaderService_SQL.GetOrdersList(applicationDbContext);

        switch (status)
        {
            case "cancelled":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusCancelled).ToList();
                break;
            case "completed":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusCompleted).ToList();
                break;
            case "refunded":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusRefunded).ToList();
                break;
            case "all":
                OrderHeader.Where(o => o.OrderStatus == SD.StatusCompleted && o.OrderStatus == SD.StatusCancelled && o.OrderStatus == SD.StatusRefunded).ToList();
                break;
            default:
                break;
        }
    }
}

