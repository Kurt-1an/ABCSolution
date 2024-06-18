using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;
public partial class OrderHeaderService_SQL : ComponentBase
{
    #region DICTIONARIES 
    #endregion

    #region FIELDS
    public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region ORDERHEADER CRUD

	public async Task<List<OrderHeader>> GetOrdersList(DbContext DBContext)
	{
		List<OrderHeader> OrdersList = new List<OrderHeader>();
		try
		{
			OrdersList = await GetOrdersListData(DBContext);

            return OrdersList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return OrdersList;
		}
	}

    public async Task<List<OrderHeader>> GetFilterOrdersList(DbContext DBContext, string userId = "", DateTime startDate = default(DateTime), DateTime endDate = default(DateTime))
    {
        List<OrderHeader> OrdersList = new List<OrderHeader>();
        try
        {
            OrdersList = await GetFilterOrdersListData(DBContext, userId, startDate, endDate);

            return OrdersList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return OrdersList;
        }
    }

    public async Task<List<OrderHeader>> GetOrderHeaderByUserId(dynamic DBContext, string userId)
	{
		List<OrderHeader> orderHeaders = new List<OrderHeader>();
		try
		{
			orderHeaders = await GetOrderHeaderByUserIdData(DBContext, userId);


			// Load ApplicationUser data for each OrderHeader
			foreach (var order in orderHeaders)
			{
				order.ApplicationUser = await DBContext.ApplicationUsers.FindAsync(order.ApplicationUserId);
			}
			return orderHeaders;

		}
		catch (Exception ex)
		{
			// Log errors
			Console.WriteLine(ex.ToString());
		}

		return orderHeaders;
	}

	public async Task<List<OrderHeader>> GetOrderHeaderByCustomerId(dynamic DBContext, Guid userId)
	{
		List<OrderHeader> orderHeaders = new List<OrderHeader>();
		try
		{
			orderHeaders = await GetOrderHeaderByCustomerIdData(DBContext, userId);


			// Load ApplicationUser data for each OrderHeader
			foreach (var order in orderHeaders)
			{
				order.ApplicationUser = await DBContext.ApplicationUsers.FindAsync(order.CustomerId);
			}
			return orderHeaders;

		}
		catch (Exception ex)
		{
			// Log errors
			Console.WriteLine(ex.ToString());
		}

		return orderHeaders;
	}

	//Get Single OrderHeader from the list of OrderHeaders
	public async Task<OrderHeader> GetOrderHeader(dynamic DBContext, int orderId)
	{
		OrderHeader OrderHeader = new();
		try
		{
			OrderHeader = await GetOrderHeaderData(DBContext, orderId);

			OrderHeader.ApplicationUser = await DBContext.ApplicationUsers.FindAsync(OrderHeader.ApplicationUserId);
			OrderHeader.Customer = await DBContext.Customers.FindAsync(OrderHeader.CustomerId);

			return OrderHeader;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return OrderHeader;
		}
	}


	//Collection of OrderDetails based on OrderHeader id
	public async Task<List<OrderDetail>> GetOrderDetailsList(dynamic DBContext, int id)
	{
		List<OrderDetail> OrderDetailsList = [];
		try
		{
			OrderDetailsList = await GetOrderDetailsListData(DBContext, id);
			return OrderDetailsList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return OrderDetailsList;
		}
	}

	public async Task<List<OrderDetail>> GetOrderDetailList(dynamic DBContext)
	{
		List<OrderDetail> OrdersList = [];

		try
		{
			OrdersList = await GetOrderDetailListData(DBContext);

			return OrdersList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return OrdersList;
		}
	}

	//GET SINGLE Order detail based on product id from the list of OrderDetails
	public async Task<OrderDetail> GetOrderDetail(dynamic DBContext, int productId)
	{
		OrderDetail _OrderDetail = new();
		try
		{
			_OrderDetail = await GetOrderDetailData(DBContext, productId);
			// Load Product data for the OrderDetail
			_OrderDetail.Product = await DBContext.Products.FindAsync(_OrderDetail.ProductId);
			return _OrderDetail;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _OrderDetail;
		}
	}

	//ADD OrderHeader
	public async Task<bool> AddOrderHeader(dynamic DBContext, OrderHeader order, ProductService_SQL productService_SQL)
	{
		bool added = false;
		try
		{
			foreach (var product in order.OrderDetails)
			{
				// GETTING THE PRODUCT INFO
				//var result2 = await productService_SQL.GetProductInfo(DBContext, product.ProductId);
				var result2 = await productService_SQL.GetStockperStoreInfo(DBContext, product.ProductId);

				if (result2 != null)
				{
					// IDENTIFY THE REQUESTING STORE AND DEDUCT THE QUANTITY
					if (order.StoreName.Contains("Addsome"))
					{
						if (result2.Store1StockQty != null)
						{
							result2.Store1StockQty -= product.Count;
						}
					}
					else if (order.StoreName.Contains("Ahead"))
					{
						if (result2.Store2StockQty != null)
						{
							result2.Store2StockQty -= product.Count;
						}
					}
					else if (String.IsNullOrEmpty(order.StoreName))
					{
						if (result2.TotalStocks != null)
						{
							result2.TotalStocks -= product.Count;
						}
					}

					// UPDATE
					await productService_SQL.UpdateStockPerStore(DBContext, result2, String.Empty);
				}
			}

			added = await AddOrderHeaderData(DBContext, order);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	//ADD OrderDetail
	public async Task<bool> AddOrderDetail(dynamic DBContext, List<OrderDetail> order)
	{
		bool added = false;
		try
		{
			added = await AddOrderDetailData(DBContext, order);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE Order
	public async Task<bool> UpdateOrderHeaderStatus(dynamic DBContext, OrderHeader order)
	{
		bool updated = false;
		try
		{
			updated = await UpdateOrderHeaderStatusData(DBContext, order);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// UPDATE Order Details
	public async Task<bool> UpdateOrderDetailStatus(dynamic DBContext, OrderDetail order)
	{
		bool updated = false;
		try
		{
			updated = await UpdateOrderDetailStatusData(DBContext, order);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}
	#endregion
}