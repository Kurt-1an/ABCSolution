using ABC.Client.Data;
using ABC.Shared.Models;
using ABC.Shared.Models.ViewModels;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Reflection.Emit;
using ABC.Shared.Utility;
using System.Runtime.InteropServices;
using System.Globalization;

namespace ABC.Client.Components.Pages.SalesInventory.SalesRecord.component;

public partial class SalesDetails
{
	#region DEPENDENCY INJECTIOn
	[Inject] ApplicationDbContext applicationDbContext { get; set; }
	[Inject] OrderHeaderService_SQL orderHeaderService_SQL { get; set; }
	#endregion

	#region fields
	private Toast toastRef;
	private List<OrderDetail> orderDetailsList { get; set; } = [];
	private OrderHeader orderHeader { get; set; } = new();

	[SupplyParameterFromQuery(Name = "id")]
	public int OrderId { get; set; }

    #endregion

    #region Variables
    private string? firstName;
    private string? phoneNumber;
    private string? lineAddress;
    private string? city;
    private string? province;
    private string? zipCode;
    private string? email;
    private decimal charges;
    private double TotalToPay;

    #endregion

    protected override async Task OnInitializedAsync()
	{
		orderHeaderService_SQL.AbcDbConnection = AppSettingsHelper.AbcDbConnection;

		orderDetailsList = await orderHeaderService_SQL.GetOrderDetailsList(applicationDbContext, OrderId);
		await LoadProducts();
	}
	private async Task LoadProducts()
	{
		orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
        if (orderHeader.ApplicationUser != null)
        {
            firstName = orderHeader.ApplicationUser.FirstName;
            phoneNumber = orderHeader.ContactNumber;
            lineAddress = orderHeader.AddressLine;
            city = orderHeader.City;
            province = orderHeader.Province;
            zipCode = orderHeader.ZipCode;
            email = orderHeader.ApplicationUser.Email;
        }
        else if (orderHeader.Customer != null)
        {
            firstName = orderHeader.Customer.FirstName;
            phoneNumber = orderHeader.ContactNumber;
            lineAddress = orderHeader.AddressLine;
            city = orderHeader.City;
            province = orderHeader.Province;
            zipCode = orderHeader.ZipCode;
            email = orderHeader.Customer.EmailAddress;
        }
        charges = orderHeader.DeliveryFee + orderHeader.ServiceFee;
        TotalToPay = orderHeader.OrderTotal + Convert.ToDouble(orderHeader.DeliveryFee);
    }

	private async Task SaveOrder()
	{

	}

	private async Task OrderPaid()
	{
		orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
		orderHeader.OrderStatus = SD.StatusCompleted;
		orderHeader.PaymentStatus = SD.StatusCompleted;

		// Call service to update OrderHeader
		bool updated = await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		if (updated)
		{
			//refresh the list
			StateHasChanged();
		}
	}

	public string FormatCurrency(double? value)
	{
		if (value.HasValue)
		{
			return value.Value.ToString("C", new CultureInfo("en-PH"));
		}
		else
		{
			return string.Empty;
		}
	}

	private async Task CancelOrder()
	{
		orderHeader = await orderHeaderService_SQL.GetOrderHeader(applicationDbContext, OrderId);
		orderHeader.OrderStatus = SD.StatusCancelled;

		// Call service to update OrderHeader
		bool updated = await orderHeaderService_SQL.UpdateOrderHeaderStatus(applicationDbContext, orderHeader);

		if (updated)
		{
			//refresh the list
			StateHasChanged();
		}
	}
}
