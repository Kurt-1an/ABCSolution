using ABC.Client.Components.Pages.SalesInventory.ProductPage.pdf;

using ABC.Client.Data;

using ABC.Client.Components;

using ABC.Shared.Models;

using ABC.Shared.Utility;

using ABC.Shared.Models.ViewModels;

using ABC.Shared.Services;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;

using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ABC.Client.Components.Pages.ShopWeb.Home;

public partial class Details

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



    private Toast toastRef;

    private string userId;

    public Product Product { get; set; } = new();

    public ShoppingCart ShoppingCart { get; set; } = new();

    public ApplicationUser ApplicationUser { get; set; } = new();

    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;

        await LoadProducts();
    }

    private async Task LoadProducts()
    {

        Product = await productService_SQL.GetProductInfo(applicationDbContext, ProductId);

        ////Default Shopping Cart quantity
        ShoppingCart = new()
        {
            Product = Product,
            ProductId = ProductId,
            Quantity = 1,
            ProductName = ProductName
        };

    }

    [Authorize]
    private async Task SaveShoppingCart()

    {
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync())?.User;

        if (string.IsNullOrEmpty(user.Identity.Name))
        {
            NavigationManager.NavigateTo($"Account/Login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}", forceLoad: true);
            return;
        }

        var claimsIdentity = user.Identity as ClaimsIdentity;
        userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);


        ShoppingCart.ApplicationUserId = userId;
        ShoppingCart.ProductName = Product.productName;


        if (ShoppingCart.Quantity > Product.StockPerStore.TotalStocks)
        {
            toastRef.ShowToast("Warning", "Cannot add more items than available in stock.");
            return;
        }


        ShoppingCart cart = await applicationDbContext.ShoppingCarts.FirstOrDefaultAsync(c => c.ApplicationUserId == userId && c.ProductId == ProductId);


        if (cart != null)
        {
            // Shopping Cart exists
            cart.Quantity += ShoppingCart.Quantity;

            //check again the stocks in inventory
            if (cart.Quantity > Product.StockPerStore.TotalStocks)
            {
                toastRef.ShowToast("Warning", "Cannot add more items than available in stock.");
                return;
            }

            toastRef.ShowToast("Success", "Updated cart successfully");
            await shoppingCartService_SQL.UpdateShoppingCart(applicationDbContext, cart);
        }


        else
        {
            await shoppingCartService_SQL.AddShoppingCart(applicationDbContext, ShoppingCart);
            toastRef.ShowToast("Success", "Product Added to cart successfully");
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
        NavigationManager.NavigateTo("/Shop", true);
    }
}