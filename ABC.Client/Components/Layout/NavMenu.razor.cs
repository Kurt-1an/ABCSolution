using ABC.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using ABC.Shared.Services;

namespace ABC.Client.Components.Layout;
public partial class NavMenu
{
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private string? currentUrl;
    private int CartItemCount = 0;
    private string userId;


    protected override async void OnInitialized()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
        await CountCartItem();
    }

    private async Task CountCartItem()
    {
        // Get current authenticated user
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync())?.User;


        if (user.Identity.IsAuthenticated)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        var cartItems = await shoppingCartService_SQL.GetShoppingCartList(applicationDbContext, userId);
        CartItemCount = cartItems.Count();
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    public void Logout()
    {
        NavigationManager.NavigateTo("/");
    }

}

