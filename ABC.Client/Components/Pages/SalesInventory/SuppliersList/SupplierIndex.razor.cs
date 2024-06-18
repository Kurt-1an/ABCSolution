using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ABC.Client.Components.Pages.SalesInventory.SuppliersList;

public partial class SupplierIndex
{
    #region INJECTIONS
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] SupplierService_SQL supplierService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    #endregion

    #region FIELDS
    private List<Supplier> Suppliers { get; set; } = [];
    private Supplier selectedSupplier { get; set; } = new();
    private String SupplierSearchInput { get; set; } = String.Empty;
    private ApplicationUser ApplicationUser { get; set; } = new();
    #endregion

    protected override async Task OnInitializedAsync()
    {
        supplierService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await GetUserBasicInfo();
        await LoadSuppliers();
    }

    private async Task LoadSuppliers()
    {
        Suppliers = await supplierService_SQL.GetSupplierList(applicationDbContext);
    }

    private async Task GetUserBasicInfo()
    {
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var claimsIdentity = user.Identity as ClaimsIdentity;
        string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);
    }

    private async Task GetSuppliersList(ChangeEventArgs e)
    {
        SupplierSearchInput = e?.Value?.ToString();

        var result = await supplierService_SQL.GetSupplierList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SupplierSearchInput))
        {
            Suppliers = result.Where(x => x.supplierCompanyName.ToString().Contains(SupplierSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.supplierEmail.ToString().Contains(SupplierSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.supplierContactNumber.ToString().Contains(SupplierSearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        else
        {
            Suppliers = result.ToList();
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task PopulateSupplierDetails(int supplierId)
    {
        selectedSupplier = new();

        var result = await supplierService_SQL.GetSupplierInfo(applicationDbContext, supplierId);

        if (result is not null)
        {
            selectedSupplier = result;
        }

        await InvokeAsync(StateHasChanged);
    }

	private async Task Archive()
	{
		selectedSupplier.supplierStatus = "Inactive";

		bool removed = await supplierService_SQL.ArchiveSupplier(applicationDbContext, selectedSupplier, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");

		if (removed)
		{
			//refresh the list
			await LoadSuppliers();
		}
	}

    private async Task Reactivate()
    {
        selectedSupplier.supplierStatus = "Active";

        bool reactivated = await supplierService_SQL.ReactivateSupplier(applicationDbContext, selectedSupplier, $"{ApplicationUser.FirstName} {ApplicationUser.LastName}");

        if (reactivated)
        {
            // Refresh the list
            await LoadSuppliers();
        }
    }

}
