using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ABC.Client.Components.Pages.SalesInventory.EmployeesList;

public partial class EmployeesList
{
    #region INJECTIONS
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }

    #endregion
    #region FIELDS
    private List<ApplicationUser> employeeUsers { get; set; } = [];
    private String EmployeeSearchInput { get; set; } = String.Empty;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        applicationUserService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await LoadEmployees();
    }

    private async Task LoadEmployees()
    {
        var users = await applicationUserService_SQL.GetApplicationUserList(applicationDbContext);
        employeeUsers = users.Where(u => u.Role != SD.Role_Customer).ToList();
    }

    private async Task GetEmployeeList(ChangeEventArgs e)
    {
        EmployeeSearchInput = e?.Value?.ToString();

        var result = await applicationUserService_SQL.GetApplicationUserList(applicationDbContext);
        employeeUsers = result.Where(u => u.Role != SD.Role_Customer).ToList();

        if (!string.IsNullOrEmpty(EmployeeSearchInput))
        {
            employeeUsers = employeeUsers.Where(x =>
                x.FirstName.Contains(EmployeeSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.LastName.Contains(EmployeeSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Role.Contains(EmployeeSearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                (x.StoreName != null && x.StoreName.Contains(EmployeeSearchInput, StringComparison.CurrentCultureIgnoreCase))
            ).ToList();
        }
        else
        {
            employeeUsers = employeeUsers.ToList();
        }

        await InvokeAsync(StateHasChanged);
    }



}