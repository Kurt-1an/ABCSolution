using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABC.Client.Components.Pages.ShopWeb.Home;

public partial class Privacy
{
    #region Injections
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ContentService_SQL ContentService_SQL { get; set; }
    #endregion

    #region Fields
    private Content content { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        content = await ContentService_SQL.GetContentInfo(applicationDbContext, 1);
    }
}
