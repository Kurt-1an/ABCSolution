using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Serilog;
using System.Security.Claims;

namespace ABC.Client.Components.Pages.SalesInventory.Audit_Log;

public partial class StockTransferLog
{
    #region DEPENDENCY INJECTIOn
    [Inject] POSService_SQL pOSService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] AuditService_SQL auditService_SQL { get; set; }
    #endregion

    #region FIELDS
    private List<StockTransferAudit> stockAudit { get; set; } = [];
    private String SearchInput { get; set; } = String.Empty;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        stockAudit = await auditService_SQL.GetStockAuditList(applicationDbContext);
        pOSService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
    }

    private async Task SearchFeature(ChangeEventArgs e)
    {
        SearchInput = e?.Value?.ToString();

        var result = await auditService_SQL.GetStockAuditList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SearchInput))
        {
            stockAudit = result.Where(x =>
                (x.Id.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Action.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.Timestamp.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.SourceStoreName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.DescitnationStoreName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.StockPerStore.Product.productName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.TransferredStocks.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
                x.EmployeeName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase))).ToList();
        }
        else
        {
            stockAudit = result?.ToList() ?? new List<StockTransferAudit>();
        }
        await InvokeAsync(StateHasChanged);
    }
}
