using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Security.Claims;
using System;
using ABC.Shared.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace ABC.Client.Components.Pages.ShopWeb.Cart.OrderCheckout;
public partial class Summary
{
    #region Injections
    [Inject] IHttpContextAccessor httpContextAccessor { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ShoppingCartService_SQL shoppingCartService_SQL { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    [Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
	[Inject] IJSRuntime JSRuntime { get; set; }
	[Inject] ContentService_SQL ContentService_SQL { get; set; }

	#endregion

	#region Fields
	[CascadingParameter]
    public HttpContext? HttpContext { get; set; }
    private List<ShoppingCart> shoppingCartList { get; set; } = [];
    public ShoppingCart cart { get; set; }
    public ShoppingCartVM shoppingCart { get; set; } = new();
    public ApplicationUser UserInfo { get; set; }
    public Product product { get; set; }


    [SupplyParameterFromForm]
    public ShoppingCartVM checkoutFormModel { get; set; }

    private string userId;
    private Toast toastRef;
	bool isChecked = false;
	private Content Content { get; set; } = new();

	private double DeliveryFee;
	private double OrderTotal;
	private double TotalPrice;
	#endregion

	protected override async Task OnInitializedAsync()
    {
        checkoutFormModel ??= new();

        // Get current authenticated user
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        // Get user by id
        UserInfo = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);
        shoppingCartList = await shoppingCartService_SQL.GetShoppingCartList(applicationDbContext, userId);
        DisplayOrderTotal();
	}

    private async Task DisplayOrderTotal()
    {
		// Set the delivery fee based on the user's city
		if (LocationData.DeliveryFees.ContainsKey(UserInfo.City))
		{
			shoppingCart.OrderHeader.DeliveryFee = LocationData.DeliveryFees[UserInfo.City];
			DeliveryFee = (double)shoppingCart.OrderHeader.DeliveryFee;
		}
		else
		{
			shoppingCart.OrderHeader.DeliveryFee = 200.0m; // default fee
			DeliveryFee = (double)shoppingCart.OrderHeader.DeliveryFee;
		}

		foreach (var cart in shoppingCartList)
		{
			cart.Price = GetPrice(cart);
			OrderTotal += (cart.Price * cart.Quantity);
		}
		TotalPrice = OrderTotal + DeliveryFee;
		shoppingCart.OrderHeader.OrderTotal += DeliveryFee;
	}

    private async Task PlaceOrderHandler()
    {

        ShoppingCartVM summary = new()
        {
            OrderHeader = new()
            {
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3),
                OrderTotal = 0,
                OrderStatus = SD.StatusPending,
                PaymentStatus = SD.PaymentStatusPending,
                Discount = 0,
                ServiceFee = 0,
                PaymentMode = SD.PaymentModeCOD,
                SalesChannel = SD.ShopWeb,
                ApplicationUserId = userId,
                ApplicationUser = UserInfo,
                ContactNumber = UserInfo.PhoneNumber,
                AddressLine = UserInfo.Address,
                City = UserInfo.City,
                Province = UserInfo.Province,
                ZipCode = UserInfo.PostalCode,
				DeliveryFee = Convert.ToDecimal(DeliveryFee),
				StoreName = "Addsome Business Corporation"
            }
        };

        // Calculate OrderTotal
        foreach (var cart in shoppingCartList)
        {
            cart.Price = GetPrice(cart);
            summary.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
        }


        summary.OrderDetailsList = new();
        foreach (var item in shoppingCartList)
        {
            OrderDetail _orderDetail = new()
            {
                ProductId = item.ProductId,
                OrderHeaderId = summary.OrderHeader.Id,
                RetailPrice = item.Price,
                Count = item.Quantity
            };

            summary.OrderDetailsList.Add(_orderDetail);
        }
        summary.OrderHeader.OrderDetails = summary.OrderDetailsList;
        shoppingCart = summary;

        bool addedOrderHeader = await orderHeaderService_SQL.AddOrderHeader(applicationDbContext, summary.OrderHeader, productService_SQL);
        //bool addedOrderDetail = await orderHeaderService_SQL.AddOrderDetail(applicationDbContext, summary.OrderDetailsList);

        foreach (var cartItem in shoppingCartList)
        {
            bool removed = await shoppingCartService_SQL.RemoveShoppingCart(applicationDbContext, cartItem);
        }

        NavigationManager.NavigateTo("/OrderConfirmation", true);
    }

    private double GetPrice(ShoppingCart shoppingCart)
    {
        return shoppingCart.Product.RetailPrice;
    }

	private async Task LoadTerms()
	{
		Content = await ContentService_SQL.GetContentInfo(applicationDbContext, 1);
	}

	private async Task No()
	{
		isChecked = false;
	}
}
