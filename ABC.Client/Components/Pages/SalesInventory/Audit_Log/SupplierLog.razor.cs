using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.Audit_Log;

public partial class SupplierLog
{
    #region DEPENDENCY INJECTIOn
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] AuditService_SQL auditService_SQL { get; set; }
    #endregion

    #region FIELDS
    private List<SupplierAudit> AuditList { get; set; } = [];
    private String SearchInput { get; set; } = String.Empty;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        AuditList = await auditService_SQL.GetSupplierAuditList(applicationDbContext);
    }

    private async Task SearchFeature(ChangeEventArgs e)
    {
        SearchInput = e?.Value?.ToString();

        var result = await auditService_SQL.GetSupplierAuditList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SearchInput))
        {
            AuditList = result.Where(x =>
                (x.Id.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Action.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Timestamp.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Supplier.supplierCompanyName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.EmployeeName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase))).ToList();
        }
        else
        {
            AuditList = result?.ToList() ?? new List<SupplierAudit>();
        }
        await InvokeAsync(StateHasChanged);
    }
}