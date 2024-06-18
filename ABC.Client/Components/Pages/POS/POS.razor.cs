using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABC.Shared.Models;
using ABC.Shared.Services;
using Serilog;
using Microsoft.AspNetCore.Components;
using ABC.Client.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ABC.Shared.Utility;
using ABC.Client.Components.Pages.ShopWeb.Cart.OrderCheckout;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;

namespace ABC.Client.Components.Pages.POS;
public partial class POS
{
    #region DEPENDENCY INJECTIOn
    [Inject] POSService_SQL pOSService_SQL { get; set; }
    [Inject] ProductService_SQL ProductService_SQL { get; set; }
    [Inject] OrderHeaderService_SQL OrderHeaderService_SQL { get; set; }
    [Inject] ApplicationDbContext applicationDbContext { get; set; }
    [Inject] UserManager<ApplicationUser> UserManager { get; set; }
	[Inject] StoreService_SQL storeService_SQL { get; set; }
	[Inject] CustomerService_SQL CustomerService_SQL { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] ApplicationUserService_SQL applicationUserService_SQL { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    #endregion

    #region FIELDS
    private ApplicationUser ApplicationUser { get; set; } = new();
    private CancellationTokenSource _tokenSource = null;
    private List<CustomerBasicInfo> CustomerList { get; set; } = [];
    private List<Product> ProductList { get; set; } = [];
    private List<Store> storeList { get; set; } = [];
    private List<OrderDetail> ShoppingCart { get; set; } = [];
    private Product ProductInModal { get; set; } = new();
    private OrderHeader OrderSummary { get; set; } = new();
    private CustomerData Customer { get; set; } = new();
    private decimal TotalFees { get; set; } = 0;
    private String ProductSearchInput { get; set; } = String.Empty;
    private Discount discount { get; set; } = new();
    private DiscountModel DiscountTypes { get; set; } = new();
    private int ItemPostRemovalId { get; set; } = 0;
    private bool ShowDropdown { get; set; } = false;
    private bool ShowProductDropdown { get; set; } = false;
    private double AmountTendered { get; set; }
    private double Change { get; set; }
    private string? userId { get; set; } = "";
    private Toast toastRef { get; set; }
    private int StockDeficit { get; set; } = 0;
    private bool ShowStockTransferAlert { get; set; } = false;
    private string SelectedSalesChannel { get; set; }
	#endregion
	protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var claimsIdentity = user.Identity as ClaimsIdentity;
        userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser = await applicationUserService_SQL.GetApplicationUserInfo(applicationDbContext, userId);

        storeList = await storeService_SQL.GetStoreList(applicationDbContext);

        pOSService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;
    }

    private async Task ShowDropdownHandler(bool show)
    {
        await Task.Delay(1000);
        ShowDropdown = show;
        StateHasChanged();
    }

    private async Task GetCustomerList(ChangeEventArgs e)
    {
        Customer.ContactNumber = e?.Value?.ToString();
        CustomerList.Clear();

        var result = await pOSService_SQL.GetCustomerList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(Customer.ContactNumber))
        {
            foreach (var item in result)
            {
                if (item.ContactNumber.ToString().StartsWith(Customer.ContactNumber, StringComparison.CurrentCultureIgnoreCase))
                {
                    CustomerBasicInfo customerBasicInfo = new()
                    {
                        Id = item.Id.ToString(),
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        ContactNumber = Convert.ToInt64(item.ContactNumber)
                    };
                    CustomerList.Add(customerBasicInfo);
                }
            }
        }
        await InvokeAsync(StateHasChanged);
    }
    private async Task PopulateFormData(string id)
    {
        var result = await pOSService_SQL.GetCustomerInfo(applicationDbContext, id);
        Customer = new()
        {
            Id = result.Id,
            FirstName = result.FirstName,
            LastName = result.LastName,
            EmailAddress = result.EmailAddress,
            ContactNumber = result.ContactNumber.ToString(),
            //Type = result.Type,
            ApSuUn = result.ApSuUn,
            StreetorSubd = result.StreetorSubd,
            Barangay = result.Barangay,
            City = result.City,
            Province = result.Province,
            ZipCode = result.ZipCode,
        };
    }

    private async Task GetProductList(ChangeEventArgs e)
    {
        ProductSearchInput = e?.Value?.ToString();
        ProductList.Clear();

        var result = await pOSService_SQL.GetProductList(applicationDbContext);
        if (result is not null && result.Count > 0 && !String.IsNullOrEmpty(ProductSearchInput))
        {
            if (ApplicationUser.StoreName.Contains("Addsome"))
            {
                ProductList = result.Where(x => x.productName.ToString().StartsWith(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase) && x.StockPerStore.Store1StockQty > 0).ToList();
            } else if (ApplicationUser.StoreName.Contains("Ahead"))
            {
                ProductList = result.Where(x => x.productName.ToString().StartsWith(ProductSearchInput, StringComparison.CurrentCultureIgnoreCase) && x.StockPerStore.Store2StockQty > 0).ToList();
            }
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowProductDropdownHandler(bool show)
    {
        await Task.Delay(1000);
        ShowProductDropdown = show;
        StateHasChanged();
    }

    private async Task PopulateProductDetails(int productId)
    {
        ProductInModal = new();
        // Find the Item
        var result = await pOSService_SQL.GetProductInfo(applicationDbContext, productId);
        if (result is not null)
        {
            ProductInModal = result;
        }
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
            existingItem.Count = quantity;
            existingItem.TotalPrice = existingItem.Count * existingItem.RetailPrice;
        }
        else
        {
            // Add the new item to the cart
            var result = await pOSService_SQL.GetProductInfo(applicationDbContext, productId);

            OrderDetail newItem = new();

            if (result != null)
            {
                newItem = new()
                {
                    Id = result.Id,
                    Product = result,
                    Count = quantity,
                    RetailPrice = result.RetailPrice,
                    CostPrice = result.CostPrice,
                    TotalPrice = quantity * result.RetailPrice
                };
            }

            // IDENTIFY THE CURRENT STORE
            var userResult = await applicationUserService_SQL.GetCurrentUserInfo(applicationDbContext, userId);

            // GET THE CURRENTSTORE QUANTITY
            int currentStoreStock = userResult.StoreName.Contains("Addsome") ? result.StockPerStore.Store1StockQty : result.StockPerStore.Store2StockQty;

            // LOGIC TO COMPARE THE CLIENT QUANTITY vs THE STORE STOCK
            if (newItem.Count > currentStoreStock)
            {
                ShowStockTransferAlert = true;
                await Task.Delay(3000).ContinueWith( _ => {
                    ShowStockTransferAlert = false;
                    InvokeAsync(StateHasChanged);
                });
            }
            else
            {
                ShoppingCart.Add(newItem);
            }
        }

        ProductSearchInput = "";
        await UpdateOrderSummary();
        await InvokeAsync(StateHasChanged);
    }

    //private async Task<bool> TransferStocks(StockPerStore stockPerStore)
    //{
    //    bool isSuccess = false;
    //    try
    //    {
    //        isSuccess = await ProductService_SQL.TransferStock(applicationDbContext, stockPerStore);
    //        return isSuccess;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex.ToString());
    //        return isSuccess;
    //    }
    //}
    public void CancelFees()
    {
        OrderSummary.ServiceFee = 0;
        OrderSummary.DeliveryFee = 0;

        UpdateTotalFees();
    }

    private async Task ItemPostRemoval(int productId)
    {
        ItemPostRemovalId = productId;
    }

    private async Task RemoveFromCart()
    {
        var item = ShoppingCart.FirstOrDefault(x => x.Product.Id == ItemPostRemovalId);
        if (item != null && item.Count > 0)
        {
            ShoppingCart.Remove(item);
            await UpdateOrderSummary();
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task UpdateTotalFees()
    {
        TotalFees = OrderSummary.ServiceFee + OrderSummary.DeliveryFee;
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdatePostTotalDiscount(string discountType = "percent")
    {
        decimal orderTotal = 0;
        foreach (var product in ShoppingCart)
        {
            orderTotal += Convert.ToDecimal(product.TotalPrice);
        }

        if (String.IsNullOrEmpty(discountType))
        {
            discountType = "percent";
        }
        discount.DiscountType = discountType;
        if (discountType == "percent")
        {
            discount.TotalDiscount = Convert.ToDouble(orderTotal) * (discount.DiscountAmount / 100);
        }
        else
        {
            discount.TotalDiscount = discount.DiscountAmount;
        }
        discount.DiscountedPrice = Convert.ToDouble(orderTotal) - discount.TotalDiscount;
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateTotalDiscount()
    {
        OrderSummary.Discount = Convert.ToDecimal(discount.TotalDiscount);
        await UpdateOrderSummary();
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateOrderSummary()
    {
        decimal orderTotal = 0;
        foreach (var product in ShoppingCart)
        {
            orderTotal += Convert.ToDecimal(product.TotalPrice);
        }
        orderTotal += (OrderSummary.ServiceFee + OrderSummary.DeliveryFee) - OrderSummary.Discount;
        OrderSummary.OrderTotal = Convert.ToDouble(orderTotal);
    }

    private async Task PaymentMethodHandler(string PaymentMethod)
    {
        switch (PaymentMethod)
        {
            case SD.PaymentModeBank:
                OrderSummary.PaymentMode = SD.PaymentModeBank;
                break;
            default:
                OrderSummary.PaymentMode = SD.PaymentModeCash;
                break;
        }
    }
    private void CalculateChange(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value.ToString(), out double amountTendered))
        {
            AmountTendered = amountTendered;
            Change = OrderSummary.OrderTotal - AmountTendered;
        }
        else
        {
            Change = 0;
        }
    }
    private void ClearCashModal()
    {
        AmountTendered = 0;
        Change = 0;
    }
    private void ClearGcashModal()
    {
        AmountTendered = 0;
        Change = 0;
    }
    private void SelectedSalesChannelChanged(ChangeEventArgs e)
    {
        if (e.Value is not null)
        {
            SelectedSalesChannel = (string)e.Value;
        }
    }

    private async Task CompleteOrder()
    {
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var user = authState.User;

		Customer customer = new()
        {
            Id = Customer.Id,
            FirstName = Customer.FirstName,
            LastName = Customer.LastName,
            EmailAddress = Customer.EmailAddress,
            ContactNumber = Convert.ToInt64(Customer.ContactNumber),
            ApSuUn = Customer.ApSuUn,
            StreetorSubd = Customer.StreetorSubd,
            Barangay = Customer.Barangay,
            City = Customer.City,
            Province = Customer.Province,
            ZipCode = Customer.ZipCode
        };
        Customer _customer = await CustomerService_SQL.GetCustomerInfo(applicationDbContext, Customer.Id);
        if (_customer.ContactNumber == 0)
        {
            bool newCustomer = await CustomerService_SQL.AddCustomer(applicationDbContext, customer);
            _customer = await CustomerService_SQL.GetCustomerInfo(applicationDbContext, Customer.Id);
        }


        OrderHeader _orderHeader = new()
        {
            OrderDate = DateTime.UtcNow.ToLocalTime(),
            ShippingDate = DateTime.UtcNow.ToLocalTime().AddDays(2),
            CompletionDate = DateTime.UtcNow.ToLocalTime(),
            OrderTotal = OrderSummary.OrderTotal,
            AmountTendered = AmountTendered,
            OrderStatus = SD.StatusCompleted,
            PaymentStatus = SD.PaymentStatusApproved,
            Carrier = OrderSummary.Carrier,
            Discount = OrderSummary.Discount,
            ServiceFee = OrderSummary.ServiceFee,
            DeliveryFee = OrderSummary.DeliveryFee,
            PaymentMode = OrderSummary.PaymentMode,
            OfficialReceipt = OrderSummary.OfficialReceipt,
            SalesChannel = SelectedSalesChannel,
            Customer = _customer,
            ContactNumber = Customer.ContactNumber,
            AddressLine = $"{Customer.ApSuUn} {Customer.StreetorSubd} {Customer.Barangay}",
            City = Customer.City,
            Province = Customer.Province,
            ZipCode = Customer.ZipCode.ToString(),
            StoreName = ApplicationUser.StoreName!,
            CompletedBy = user.Identity.Name
        };

        List<OrderDetail> orderDetails = [];
        foreach (var item in ShoppingCart)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = item.Id,
                OrderHeaderId = _orderHeader.Id,
                RetailPrice = item.RetailPrice,
                CostPrice = item.CostPrice,
                Count = item.Count
            };
            orderDetails.Add(orderDetail);
        }

        _orderHeader.OrderDetails = orderDetails;

        bool added = await OrderHeaderService_SQL.AddOrderHeader(applicationDbContext, _orderHeader, ProductService_SQL);

        if (added)
        {
            toastRef.ShowToast("Success", ($"Transaction #{_orderHeader.Id} completed successfully!"), $"/OrderDetails?id={_orderHeader.Id}", " Print");
            await NewTransaction();
        }
        else
        {
            toastRef.ShowToast("Error", ($"Transaction #{_orderHeader.Id} was not saved! Please fill required fields!"));
        }

        await InvokeAsync(StateHasChanged);

    }

    private async Task NewTransaction()
    {
        ShoppingCart.Clear();
        OrderSummary = new OrderHeader();
        Customer = new CustomerData();
        TotalFees = 0;
        ProductSearchInput = string.Empty;
        discount = new Discount();
        DiscountTypes = new DiscountModel();
        AmountTendered = 0;
        Change = 0;
        ShowDropdown = false;
        ShowProductDropdown = false;
        ItemPostRemovalId = 0;
    }

    private async Task CancelOrder()
    {
        ShoppingCart.Clear();
        OrderSummary = new OrderHeader();
        Customer = new CustomerData();
        TotalFees = 0;
        ProductSearchInput = string.Empty;
        discount = new Discount();
        DiscountTypes = new DiscountModel();
        AmountTendered = 0;
        Change = 0;
        ShowDropdown = false;
        ShowProductDropdown = false;
        ItemPostRemovalId = 0;
    }

    private void Print()
    {
        JSRuntime.InvokeVoidAsync("Print");
    }
}