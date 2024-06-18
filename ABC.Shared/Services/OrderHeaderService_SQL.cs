using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Mysqlx.Crud;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;

public partial class OrderHeaderService_SQL
{

    #region ORDERHEADER CRUD

    //Get List of OrderHeaders
    private async Task<List<OrderHeader>> GetOrdersListData(DbContext DBContext)
	{
        List<OrderHeader> _order = [];
        try
        {
            var context = DBContext;
            List<OrderHeader> orderList = [];

            orderList = [.. context.Set<OrderHeader>().Include(x => x.ApplicationUser).Include(x => x.Customer)];

            foreach (var item in orderList)
            {
                _order.Add(item);
            }
            return _order;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _order;
        }
    }

    private async Task<List<OrderHeader>> GetFilterOrdersListData(DbContext DBContext, string userId = "", DateTime startDate = default(DateTime), DateTime endDate = default(DateTime))
	{
		List<OrderHeader> _order = [];
		try
		{
			var context = DBContext;
			List<OrderHeader> orderList = [];
			if (!String.IsNullOrEmpty(userId))
			{
				if (startDate != default(DateTime) && endDate != default(DateTime))
				{
                    orderList = [.. context.Set<OrderHeader>()
						.Where(x => x.CompletionDate >= startDate && x.CompletionDate <= endDate)
						.Include(x => x.ApplicationUser)
						.Include(x => x.Customer)
						.Where(x => x.ApplicationUserId == userId)
                        .Include(x => x.OrderDetails).ThenInclude(o => o.Product)];
                } else
				{
                    orderList = [.. context.Set<OrderHeader>()
						.Include(x => x.ApplicationUser)
						.Include(x => x.Customer)
						.Where(x => x.ApplicationUserId == userId)
                        .Include(x => x.OrderDetails).ThenInclude(o => o.Product)];
                }
            } else {
                if (startDate != default(DateTime) && endDate != default(DateTime))
                {
                    orderList = [.. context.Set<OrderHeader>()
						.Where(x => x.CompletionDate >= startDate && x.CompletionDate <= endDate)
						.Include(x => x.ApplicationUser)
						.Include(x => x.Customer)
                        .Include(x => x.OrderDetails).ThenInclude(o => o.Product)];
                } else
				{
                    orderList = [.. context.Set<OrderHeader>()
						.Include(x => x.ApplicationUser)
						.Include(x => x.Customer)
						.Include(x => x.OrderDetails).ThenInclude(o => o.Product)];
                }
            }
			
			foreach (var item in orderList)
			{
				_order.Add(item);
			}
			return _order;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _order;
		}
	}

	private async Task<List<OrderHeader>> GetOrderHeaderByUserIdData(dynamic DBContext, string userId)
	{
		List<OrderHeader> orderHeaders = new List<OrderHeader>();
		try
		{
			var context = DBContext;
			var orderList = ((IQueryable<OrderHeader>)context.OrderHeaders).Where(o => o.ApplicationUserId == userId);


			foreach (var item in orderList)
			{
				orderHeaders.Add(item);
			}

			return orderHeaders;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return orderHeaders;
		}
	}

	private async Task<List<OrderHeader>> GetOrderHeaderByCustomerIdData(dynamic DBContext, Guid userId)
	{
		List<OrderHeader> orderHeaders = new List<OrderHeader>();
		try
		{
			var context = DBContext;
			var orderList = ((IQueryable<OrderHeader>)context.OrderHeaders).Where(o => o.CustomerId == userId);


			foreach (var item in orderList)
			{
				orderHeaders.Add(item);
			}

			return orderHeaders;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return orderHeaders;
		}
	}

	//Get Single OrderHeader from the list of OrderHeaders
	private async Task<OrderHeader> GetOrderHeaderData(dynamic DBContext, int orderId)
	{
		OrderHeader _orderHeader = new();
		try
		{
			var context = DBContext;
			var result = context.OrderHeaders.Find(orderId);
			if (result is not null)
			{
				_orderHeader = result;
			}
			return _orderHeader;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _orderHeader;
		}
	}

	//Collection of OrderDetails based on OrderHeader id
	private async Task<List<OrderDetail>> GetOrderDetailsListData(dynamic DBContext, int id)
	{
		List<OrderDetail> _orderDetails = new List<OrderDetail>();
		try
		{
            List<Product> _product = new List<Product>();
            var context = DBContext;
            var shoppingCartList = ((IQueryable<OrderDetail>)context.OrderDetails).Where(sc => sc.OrderHeaderId == id);
            var products = context.Products;

            foreach (var product in products)
            {
                _product.Add(product);
            }

            foreach (var cartItem in shoppingCartList)
            {
                OrderDetail item = cartItem;
                item.Product = _product.FirstOrDefault(p => p.Id == cartItem.ProductId);
                _orderDetails.Add(item);
            }

			return _orderDetails;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _orderDetails;
		}
	}

	private async Task<List<OrderDetail>> GetOrderDetailListData(dynamic DBContext)
	{
		List<OrderDetail> _order = [];

		try
		{
			List<Product> _product = new List<Product>();
			var context = DBContext;
			var orderList = context.OrderDetails;
			var products = context.Products;

			foreach (var product in products)
			{
				_product.Add(product);
			}

			foreach (var order in orderList)
			{
				OrderDetail item = order;
				item.Product = _product.FirstOrDefault(p => p.Id == order.ProductId);
				_order.Add(order);
			}

			return _order;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _order;
		}
	}

	//GET SINGLE Order detail based on product id from the list of OrderDetails
	private async Task<OrderDetail> GetOrderDetailData(dynamic DBContext, int productId)
	{
		OrderDetail _orderDetail = new();
		try
		{
			var context = DBContext;
			var result = context.OrderDetails.Find(productId);

			if (result is not null)
			{
				_orderDetail = result;
			}
			return _orderDetail;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _orderDetail;
		}
	}

	//ADD OrderHeader
	private async Task<bool> AddOrderHeaderData(DbContext DBContext, OrderHeader order)
	{
		try
		{
			if (order.ApplicationUser != null)
			{
                var existingUser = DBContext.Set<ApplicationUser>().Find(order.ApplicationUser.Id);
                DBContext.Entry(order.ApplicationUser).State = EntityState.Unchanged;
				//DBContext.Entry(order.ApplicationUserId).State = EntityState.Unchanged;
			} else if (order.Customer != null)
			{
                var existingCustomer = DBContext.Set<Customer>().Find(order.Customer.Id);
                DBContext.Entry(order.Customer).State = EntityState.Unchanged;
            }
			DBContext.Set<OrderHeader>().Add(order);
			var result = DBContext.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

    //ADD OrderDetails
    private async Task<bool> AddOrderDetailData(dynamic DBContext, List<OrderDetail> orderDetails)
    {
        try
        {
            var context = DBContext;

			foreach (var item in orderDetails)
			{
				context.OrderDetails.Add(item);
			}

            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    //* UPDATE Order Header status ON DB
    private async Task<bool> UpdateOrderHeaderStatusData(DbContext DBContext, OrderHeader order)
	{
		try
		{
			if (order.ApplicationUser != null)
			{
				DBContext.Entry(order.ApplicationUser).State = EntityState.Unchanged;
			}
			DBContext.Set<OrderHeader>().Update(order);
			var result = DBContext.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE Order Details
	private async Task<bool> UpdateOrderDetailStatusData(dynamic DBContext, OrderDetail order)
	{
		try
		{
			var context = DBContext;
			context.OrderDetails.Update(order);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}
	#endregion

}