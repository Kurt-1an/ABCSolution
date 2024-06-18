using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.SalesInventory.Settings.General;

public partial class GeneralSettings
{
    #region Injections
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] StoreService_SQL storeService_SQL { get; set; }
    #endregion

    #region Fields
    private List<Store> StoresList { get; set; } = [];
    #endregion

    #region Store
    protected override async Task OnInitializedAsync()
    {
        StoresList = await storeService_SQL.GetStoreList(applicationDbContext);
    }
    #endregion
}
