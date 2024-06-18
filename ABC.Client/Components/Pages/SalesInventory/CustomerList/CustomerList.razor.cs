using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.CustomerList;

public partial class CustomerList
{
    #region INJECTIONS
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] CustomerService_SQL customerService_SQL { get; set; }
    #endregion

    #region FIELDS
    private List<Customer> Customers { get; set; } = [];
    private String CustomerSearchInput { get; set; } = String.Empty;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        customerService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadCustomers();
    }

    private async Task LoadCustomers()
    {
        Customers = await customerService_SQL.GetCustomersList(applicationDbContext);
    }

    private async Task GetCustomerList(ChangeEventArgs e)
    {
        CustomerSearchInput = e?.Value?.ToString();

        var result = await customerService_SQL.GetCustomersList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(CustomerSearchInput))
        {
            Customers = result.Where(x => x.FirstName.ToString().Contains(CustomerSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.LastName.ToString().Contains(CustomerSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.ContactNumber.ToString().Contains(CustomerSearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        else
        {
            Customers = result.ToList();
        }
        await InvokeAsync(StateHasChanged);
    }
}
