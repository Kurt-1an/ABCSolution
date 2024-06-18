using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using ABC.Shared.Utility;
using System.Security.Claims;

namespace ABC.Client.Components.Pages.ShopWeb.ManageOrderPage;

public partial class ManageOrder
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

    #region fields
    private string activeStatus = "all"; // Track the active status
    private string all { get; set; } = "text-primary";
    private string pending { get; set; } = "text-primary";
    private string inprocess { get; set; } = "text-primary";
    private string shipped { get; set; } = "text-primary";
    private string cancelled { get; set; } = "text-primary";
    private string completed { get; set; } = "text-primary";


    public HttpContext? HttpContext { get; set; }
    private string userId;
    private List<OrderHeader> OrderHeader { get; set; } = new List<OrderHeader>();
    public ApplicationUser User { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadProducts();
        SetActiveStatus(activeStatus);
    }

    private async Task LoadProducts()
    {
        // Get the authenticated user's claims
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        // If the user is authenticated, retrieve their user ID from the claims
        if (user.Identity.IsAuthenticated)
        {
            userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Fetch the orders for the logged-in user
            OrderHeader = await orderHeaderService_SQL.GetOrderHeaderByUserId(applicationDbContext, userId);
        }

        FilterOrdersByStatus(activeStatus);
    }

    private async Task SetActiveStatus(string status)
    {
        activeStatus = status;
        all = "text-primary";
        pending = "text-primary";
        inprocess = "text-primary";
        shipped = "text-primary";
        cancelled = "text-primary";
        completed = "text-primary";

        switch (status)
        {
            case "pending":
                pending = "active text-white bg-primary";
                break;
            case "inprocess":
                inprocess = "active text-white bg-primary";
                break;
            case "shipped":
                shipped = "active text-white bg-primary";
                break;
            case "cancelled":
                cancelled = "active text-white bg-primary";
                break;
            case "completed":
                completed = "active text-white bg-primary";
                break;
            case "all":
                all = "active text-white bg-primary";
                break;
            default:
                all = "active text-white bg-primary";
                break;
        }

        FilterOrdersByStatus(status);
    }

    private async Task FilterOrdersByStatus(string status)
    {
        OrderHeader = await orderHeaderService_SQL.GetOrderHeaderByUserId(applicationDbContext, userId);


        switch (status)
        {
            case "pending":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusPending).ToList();
                break;
            case "inprocess":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusProcessing).ToList();
                break;
            case "shipped":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusShipped).ToList();
                break;
            case "cancelled":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusCancelled).ToList();
                break;
            case "completed":
                OrderHeader = OrderHeader.Where(o => o.OrderStatus == SD.StatusCompleted).ToList();
                break;
            default:
                break;
        }
    }
}
