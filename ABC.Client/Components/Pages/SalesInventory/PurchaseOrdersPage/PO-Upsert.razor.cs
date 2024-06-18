using ABC.Client.Components.Pages.SalesInventory.ProductPage;
using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Services;
using ABC.Shared.Utility;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Cryptography;

namespace ABC.Client.Components.Pages.SalesInventory.PurchaseOrdersPage;

public partial class PO_Upsert
{
    #region Injections
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] PurchaseOrderService_SQL purchaseOrderService_SQL { get; set; }
    [Inject] SupplierService_SQL supplierService_SQL { get; set; }
    [Inject] StoreService_SQL storeService_SQL { get; set; }
    [Inject] ProductService_SQL productService_SQL { get; set; }
    [Inject] NavigationManager navigationManager { get; set; }
	[Inject] IJSRuntime JSRuntime { get; set; }

	#endregion

	#region Fields
	private List<Product> ProductList { get; set; } = [];
    private List<Supplier> SupplierList { get; set; } = [];
    private List<Store> StoreList { get; set; } = [];

    [SupplyParameterFromQuery(Name = "id")]
    public int PurchaseOrderID { get; set; }

    [SupplyParameterFromQuery(Name = "Store")]
    public string StoreId { get; set; }

    //[SupplyParameterFromForm]
    //public PurchaseOrder purchaseOrder  { get; set; }
    private List<PurchaseOrderItem> ShoppingCart { get; set; } = [];
    [SupplyParameterFromForm]
    public PurchaseOrder selectedPurchaseOrder { get; set; } = new();
    private String ProductSearchInput { get; set; } = String.Empty;
    private Product ProductInModal { get; set; } = new();
    private Store selectedStore { get; set; } = new();
    private bool ShowProductDropdown { get; set; } = false;
    private String SearchInput { get; set; } = String.Empty;
    private int ItemPostRemovalId { get; set; } = 0;
    private bool isEditMode = false;
    private string storeName;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        selectedPurchaseOrder ??= new();
        purchaseOrderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
        await GetSuppliers();
        await LoadPurchaseOrder();
        await SetSupplierDetails();
        await GetStoreDetails();
        await InvokeAsync(StateHasChanged);

        if (PurchaseOrderID > 0)
        {
            await DisplaySelectedProducts();
        }

        if (selectedPurchaseOrder.Id == 0)
        {
            var formattedDate = DateTime.Now.AddDays(2).ToString("MMMM dd yyyy");
            selectedPurchaseOrder.DeliveryDate = DateOnly.ParseExact(formattedDate, "MMMM dd yyyy", CultureInfo.InvariantCulture);
            selectedPurchaseOrder.Status = SD.PO_Pending;
		}
        if (selectedPurchaseOrder.Id != 0)
        {
            isEditMode = true;
            storeName = selectedPurchaseOrder.Store.storeName;

		}
    }
    private async Task LoadPurchaseOrder()
    {
        var purchaseTask = purchaseOrderService_SQL.GetPurchaseOrderInfo(applicationDbContext, PurchaseOrderID);
        selectedPurchaseOrder = await purchaseTask;
    }
    private async Task SearchFeature(ChangeEventArgs e)
    {
        SearchInput = e?.Value?.ToString();

        var result = await supplierService_SQL.GetSupplierList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(SearchInput))
        {
            SupplierList = result.Where(x => x.Id.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.supplierCompanyName.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.supplierStatus.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.supplierContactNumber.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase) ||
            x.supplierEmail.ToString().Contains(SearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        else
        {
            SupplierList = result.ToList();
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task GetStoreDetails()
    {
        StoreList = await storeService_SQL.GetStoreList(applicationDbContext);
    }
    private async Task GetSuppliers()
    {
        SupplierList = await supplierService_SQL.GetSupplierList(applicationDbContext);
    }
    private async Task GetProductList(ChangeEventArgs e)
    {
        ProductSearchInput = e?.Value?.ToString();
        ProductList.Clear();

        var result = await productService_SQL.GetProductList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(ProductSearchInput))
        {
            ProductList = result
                .Where(x => x.productName.ToString().StartsWith(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task PopulateProductDetails(int productId)
    {
        ProductInModal = new();
        // Find the Item
        var result = await productService_SQL.GetProductInfo(applicationDbContext, productId);
        if (result is not null)
        {
            ProductInModal = result;
        }
    }
    private async Task SelectedStoreHandler(int storeId)
    {
        selectedStore = StoreList.FirstOrDefault(s => s.Id == storeId);
        if (selectedStore != null)
        {
            selectedPurchaseOrder.Store = selectedStore;
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task SetSupplierDetails()
    {
        int _Id;
        Supplier selectedSupplier = new();
        if (!String.IsNullOrEmpty(StoreId) && int.TryParse(StoreId, out _Id))
        {
            selectedSupplier = await supplierService_SQL.GetSupplierInfo(applicationDbContext, _Id);
            selectedPurchaseOrder.Supplier = selectedSupplier;
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task ShowProductDropdownHandler(bool show)
    {
        await Task.Delay(1000);
        ShowProductDropdown = show;
        StateHasChanged();
    }
    private async Task ItemPostRemoval(int productId)
    {
        ItemPostRemovalId = productId;
    }
    private async Task RemoveFromCart()
    {
        var item = ShoppingCart.FirstOrDefault(x => x.Product.Id == ItemPostRemovalId);
        if (item != null && item.Quantity > 0)
        {
            ShoppingCart.Remove(item);
            //await UpdateOrderSummary();
            await InvokeAsync(StateHasChanged);
        }
        await CalculateOrderTotal();
    }
    private async Task AddToCart(int productId, int quantity)
    {
        // Find the Item
        if (quantity == 0)
        {
            return;
        };

        var existingItem = ShoppingCart.FirstOrDefault(x => x.Product.Id == productId);
        if (existingItem != null)
        {
            // Update quantity directly to the new input value
            existingItem.Quantity = quantity;
            existingItem.SubTotal = existingItem.Quantity * existingItem.Cost;
        }
        else
        {
            // Add the new item to the cart
            var result = await productService_SQL.GetProductInfo(applicationDbContext, productId);
            if (result != null)
            {
                PurchaseOrderItem newItem = new()
                {
                    Product = result,
                    Quantity = quantity,
                    Cost = result.CostPrice,
                    SubTotal = quantity * result.CostPrice
                };
                ShoppingCart.Add(newItem);
            }
        }
        //selectedPurchaseOrder.PurchasedProducts.Clear();
        selectedPurchaseOrder.PurchasedProducts = ShoppingCart;
        ProductSearchInput = "";
        await CalculateOrderTotal();
        await InvokeAsync(StateHasChanged);
    }

    private async Task CalculateOrderTotal()
    {
        selectedPurchaseOrder.OrderTotal = ShoppingCart.Sum(item => item.Quantity * item.Cost);
    }

    private async Task DisplaySelectedProducts()
    {
        var purchaseOrder = await purchaseOrderService_SQL.GetPurchaseOrderList(applicationDbContext);
        var purchaseOrderItem = purchaseOrder.FirstOrDefault(x => x.Id == PurchaseOrderID);
        ShoppingCart = purchaseOrderItem.PurchasedProducts;

    }

    private async Task SavePurchaseOrder()
    {
        if (selectedPurchaseOrder.Id == 0)
        {
            bool added = await purchaseOrderService_SQL.AddPurchaseOrder(applicationDbContext, selectedPurchaseOrder);
            navigationManager.NavigateTo("/PurchaseOrdersList", true);
        }
    }
    private async Task CancelAction()
    {
        if (selectedPurchaseOrder.Id != 0)
        {
            selectedPurchaseOrder = await purchaseOrderService_SQL.GetPurchaseOrderInfo(applicationDbContext, selectedPurchaseOrder.Id);
            navigationManager.NavigateTo("/PurchaseOrdersList", true);
        }
        else
        {
            selectedPurchaseOrder = new PurchaseOrder();
            navigationManager.NavigateTo("/PurchaseOrdersList", true);
        }
    }


	private void Print()
	{
		JSRuntime.InvokeVoidAsync("Prints");
	}

}
