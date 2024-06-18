using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

namespace ABC.Client.Components.Pages.Home;
public partial class Home
{
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }


    #region fields
    private List<Product> ProductList { get; set; } = [];
    private List<Product> BestSellingProducts { get; set; } = new ();

    #endregion

    #region
    protected override async Task OnInitializedAsync()
    {
        productService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        ProductList = await productService_SQL.GetProductList(applicationDbContext);
        await LoadData();
    }

    private async Task LoadData()
    {
        BestSellingProducts = await applicationDbContext.OrderDetails
                       .Include(detail => detail.Product)
                       .GroupBy(detail => detail.Product)
                       .OrderByDescending(group => group.Sum(detail => detail.Count))
                       .Select(group => group.Key)

                       .ToListAsync();

        await InvokeAsync(StateHasChanged);
    }

    #endregion
}