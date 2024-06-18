using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ABC.Client.Components.Pages.SalesInventory.SuppliersList.upsert;

public partial class SupplierUpsert
{
    #region INJECTIONS
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] SupplierService_SQL supplierService_SQL { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    #endregion

    #region FIELDS
    private List<Supplier> Suppliers { get; set; } = [];
    private List<Store> StoreList { get; set; } = [];
    private Supplier SelectedSupplier { get; set; } = new();
    private ApplicationUser ApplicationUser { get; set; } = new();

    [SupplyParameterFromQuery(Name = "id")]
    public int supplierId { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        supplierService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await GetUserBasicInfo();
        await LoadSupplier();
    }

    private async Task GetUserBasicInfo()
    {
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var claimsIdentity = user.Identity as ClaimsIdentity;
        string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);
    }

    private async Task LoadSupplier()
    {
        var supplierTask = supplierService_SQL.GetSupplierInfo(applicationDbContext, supplierId);
        SelectedSupplier = await supplierTask;
    }

    private async Task SaveSupplier()
    {
        if (SelectedSupplier.Id == 0)
        {
            bool added = await supplierService_SQL.AddSupplier(applicationDbContext, SelectedSupplier, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");
            NavigationManager.NavigateTo("/SupplierList", true);
        } else
        {
            bool updated = await supplierService_SQL.UpdateSupplier(applicationDbContext, SelectedSupplier, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");
			NavigationManager.NavigateTo("/SupplierList", true);
		}
    }
}
