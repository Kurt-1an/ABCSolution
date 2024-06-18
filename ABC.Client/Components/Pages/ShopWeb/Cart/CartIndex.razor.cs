using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using ABC.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Diagnostics.Eventing.Reader;

namespace ABC.Client.Components.Pages.ShopWeb.Cart;
public partial class CartIndex
{

	#region DEPENDENCY INJECTION

	[Inject] ApplicationDbContext applicationDbContext { get; set; }

	[Inject] ProductService_SQL productService_SQL { get; set; }

	[Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }

	[Inject] ShoppingCartService_SQL shoppingCartService_SQL { get; set; }

	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

	[Inject] IHttpContextAccessor HttpContextAccessor { get; set; }
	[Inject] NavigationManager NavigationManager { get; set; }


	#endregion

	#region fields

	[SupplyParameterFromQuery(Name = "product")]

	public string ProductName { get; set; }

	[SupplyParameterFromQuery(Name = "id")]

	public int ProductId { get; set; }



	private List<ShoppingCart> shoppingCartList { get; set; } = [];

	private Toast toastRef;
	private string userId;
	public Product Product { get; set; } = new();
	public ShoppingCart ShoppingCart { get; set; } = new();
	public OrderHeader OrderHeader { get; set; } = new();
	public ShoppingCartVM ShoppingCartVM { get; set; } = new();

	public ApplicationUser ApplicationUser { get; set; } = new();
	#endregion

	protected override async Task OnInitializedAsync()
	{
		productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
		await LoadProducts();
	}

	private async Task LoadProducts()
	{
		// Get current authenticated user
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = authState.User;

		if (user.Identity.IsAuthenticated)
		{
			var claimsIdentity = user.Identity as ClaimsIdentity;
			userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
		}

		// Set user id to application User
		ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);

		ShoppingCartVM = new()
		{
			ShoppingCartList = await shoppingCartService_SQL.GetShoppingCartList(applicationDbContext, userId),
			OrderHeader = new()
		};

		UpdateOrderTotal();
	}

	private async Task AdjustQuantity(ShoppingCart cartItem, int quantityChange)
	{
		if ((cartItem.Quantity < cartItem.Product.StockPerStore.TotalStocks || quantityChange < 0) && (cartItem.Quantity > 1 || quantityChange > 0))
		{
			cartItem.Quantity += quantityChange;
			bool updated = await shoppingCartService_SQL.UpdateShoppingCart(applicationDbContext, cartItem);
			await InvokeAsync(StateHasChanged);
			
			if (updated)
			{
				UpdateOrderTotal();
			}
			
		} else
		{
			//Notice
			if (quantityChange > 0)
			{
				toastRef.ShowToast("Warning", "Cannot add more items beyond the available stock quantity!");
			} else
			{
				toastRef.ShowToast("Warning", "Cannot input quantity below 1");
			}
			
		}
	}

	private async Task PopulateCartDetails(int productId)
	{
		ShoppingCart = ShoppingCartVM.ShoppingCartList.FirstOrDefault(c => c.ProductId == productId);
		await InvokeAsync(StateHasChanged);
	}

	private async Task Remove(ShoppingCart cartItem)
	{
		// Call service to remove product
		bool removed = await shoppingCartService_SQL.RemoveShoppingCart(applicationDbContext, cartItem);
		if (removed)
		{
			//refresh the list
			await LoadProducts();
		}
	}

	private void UpdateOrderTotal()
	{
		ShoppingCartVM.OrderHeader.OrderTotal = ShoppingCartVM.ShoppingCartList.Sum(cart => cart.Product.RetailPrice * cart.Quantity);
		StateHasChanged(); // Refresh the view
	}


}