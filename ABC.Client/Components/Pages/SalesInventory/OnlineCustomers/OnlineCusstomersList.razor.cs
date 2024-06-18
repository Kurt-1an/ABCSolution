using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.OnlineCustomers;

public partial class OnlineCusstomersList
{
    #region INJECTIONS
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    #endregion

    #region FIELDS
    private List<ApplicationUser> OnlineCustomers { get; set; } = [];
    private String CustomerSearchInput { get; set; } = String.Empty;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        applicationUserService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadCustomers();
    }

    private async Task LoadCustomers()
    {
        var users = await applicationUserService_SQL.GetApplicationUserList(applicationDbContext);
        OnlineCustomers = users.Where(u => u.Role == SD.Role_Customer).ToList();
    }

    private async Task GetOnlineCustomerList(ChangeEventArgs e)
    {
        CustomerSearchInput = e?.Value?.ToString();

        var result = await applicationUserService_SQL.GetApplicationUserList(applicationDbContext);
        OnlineCustomers = result.Where(u => u.Role == SD.Role_Customer).ToList();

        if (!string.IsNullOrEmpty(CustomerSearchInput))
        {
            OnlineCustomers = result.Where(x =>
                x.FirstName.Contains(CustomerSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.LastName.Contains(CustomerSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                (x.PhoneNumber != null && x.PhoneNumber.Contains(CustomerSearchInput, StringComparison.CurrentCultureIgnoreCase))
            ).ToList();
        }
        else
        {
            OnlineCustomers = OnlineCustomers.ToList();
        }

        await InvokeAsync(StateHasChanged);
    }
}

