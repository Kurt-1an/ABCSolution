using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;

public partial class PurchaseOrderService_SQL
{
	#region Purchase Order CRUD
	//* GET ALL PO
	private async Task<List<PurchaseOrder>> GetPurchaseOrdersListData(dynamic DBContext)
	{
		List<PurchaseOrder> _purchaseOrdersList = [];
		try
		{
			var context = DBContext;
			var purchaseOrdersList = context.PurchaseOrders;
			IEnumerable<PurchaseOrderItem> purchaseOrderItems = context.PurchaseOrderItems;
			IEnumerable<Product> products = context.Products;
			IEnumerable<Supplier> suppliers = context.Suppliers;
			IEnumerable<Store> stores = context.Stores;
			//purchaseOrderItems.Where(x => x.);
			foreach (var item in purchaseOrdersList)
			{
				_purchaseOrdersList.Add(item);
			}

			foreach (var details in _purchaseOrdersList)
			{
				List<PurchaseOrderItem> orderItems = purchaseOrderItems.Where(x => x.PurchaseOrderId == details.Id).ToList();
				foreach (var orderItem in orderItems)
				{
					orderItem.Product = products.FirstOrDefault(p => p.Id == orderItem.ProductId);
				}
				details.PurchasedProducts = orderItems;
				details.Supplier = suppliers.FirstOrDefault(s => s.Id == details.SupplierId);
				details.Store = stores.FirstOrDefault(st => st.Id == details.StoreId);
			}
			return _purchaseOrdersList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _purchaseOrdersList;
		}
	}

	//* GETS SINGLE PO BASE ON PO ID
	private async Task<PurchaseOrder> GetPurchaseOrderData(DbContext DBContext, int id)
    {
        PurchaseOrder _purchaseOrder = new();
        try
        {
            var result = DBContext.Set<PurchaseOrder>()
                .Include(x => x.PurchasedProducts)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.StockPerStore)
                .FirstOrDefault(x => x.Id == id);

            if (result is not null)
            {
                _purchaseOrder = result;
            }
            return _purchaseOrder;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _purchaseOrder;
        }
    }

	//* ADDS PO TO DB
	private async Task<bool> AddPurchaseOrderData(dynamic DBContext, PurchaseOrder purchaseOrder)
    {
        try
        {
            var context = DBContext;
            if (purchaseOrder.PurchasedProducts != null)
            {
                foreach (var product in purchaseOrder.PurchasedProducts)
                {
                    context.Entry(product.Product).State = EntityState.Unchanged;
                }
            }
			if (purchaseOrder.Supplier != null)
			{
				context.Entry(purchaseOrder.Supplier).State = EntityState.Unchanged;
			}
			if (purchaseOrder.Store != null)
			{
				context.Entry(purchaseOrder.Store).State = EntityState.Unchanged;
			}
			context.PurchaseOrders.Add(purchaseOrder);
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

	//* UPDATE PO ON DB
	private async Task<bool> UpdatePurchaseOrderData(dynamic DBContext, PurchaseOrder purchaseOrder)
    {
        try
        {
            var context = DBContext;
            context.PurchaseOrders.Update(purchaseOrder);
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