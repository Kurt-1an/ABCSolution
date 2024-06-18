using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.Audit_Log;

public partial class ProductLog
{
    [Inject] AuditService_SQL auditService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }

    private List<ProductAudit> productAuditList { get; set; } = new List<ProductAudit>();
    private String SearchInput { get; set; } = String.Empty;

    protected override async Task OnInitializedAsync()
    {
        productAuditList = await auditService_SQL.GetProductAuditList(applicationDbContext);
    }

    private async Task SearchFeature(ChangeEventArgs e)
    {
        SearchInput = e?.Value?.ToString();

        var result = await auditService_SQL.GetProductAuditList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SearchInput))
        {
            productAuditList = result.Where(x =>
                (x.Id.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Action.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Timestamp.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.EmployeeName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Product.productName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase))).ToList();
        }
        else
        {
            productAuditList = result?.ToList() ?? new List<ProductAudit>();
        }
        await InvokeAsync(StateHasChanged);
    }
}
