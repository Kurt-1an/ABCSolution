using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;
using ABC.Shared.Utility;

namespace ABC.Shared.Services;
public partial class PurchaseOrderService_SQL : ComponentBase
{

    #region FIELDS
    public String AbcDbConnection { get; set; } = String.Empty;
    #endregion

    #region PurchaseOrders CRUD
    public async Task<PurchaseOrder> GetPurchaseOrderInfo(dynamic DBContext, int Id)
    {
        PurchaseOrder PurchaseOrderInfo = new();
        try
        {
            PurchaseOrderInfo = await GetPurchaseOrderData(DBContext, Id);
            return PurchaseOrderInfo;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return PurchaseOrderInfo;
        }
    }

    public async Task<List<PurchaseOrder>> GetPurchaseOrderList(dynamic DBContext)
    {
        List<PurchaseOrder> PurchaseOrderList = [];
        try
        {
            PurchaseOrderList = await GetPurchaseOrdersListData(DBContext);
            return PurchaseOrderList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return PurchaseOrderList;
        }
    }

    public async Task<bool> AddPurchaseOrder(dynamic DBContext, PurchaseOrder purchaseOrder)
    {
        bool added = false;
        try
        {
            added = await AddPurchaseOrderData(DBContext, purchaseOrder);
            return added;
        }
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            return added;
        }
    }

    public async Task<bool> UpdatePurchaseOrder(dynamic DBContext, PurchaseOrder purchaseOrder, ProductService_SQL productService_SQL)
    {
        bool updated = false;
        try
        {
			if (purchaseOrder.Status != SD.PO_Rejected)
			{
				foreach (var product in purchaseOrder.PurchasedProducts)
				{
					var result2 = await productService_SQL.GetProductInfo(DBContext, product.ProductId);
                    if (purchaseOrder.Store.storeName.Contains("Addsome"))
                    {
                        result2.StockPerStore.Store1StockQty += product.Quantity;
                    } else if (purchaseOrder.Store.storeName.Contains("Ahead"))
                    {
                        result2.StockPerStore.Store2StockQty += product.Quantity;
                    }

                    result2.StockPerStore.TotalStocks = result2.StockPerStore.Store1StockQty + result2.StockPerStore.Store2StockQty;
                    await productService_SQL.UpdateProduct(DBContext, result2, string.Empty);
				}
				updated = await UpdatePurchaseOrderData(DBContext, purchaseOrder);
				return updated;
			}
			else
			{
				updated = await UpdatePurchaseOrderData(DBContext, purchaseOrder);
				return updated;
			}
		}
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            return updated;
        }
    }

    public async Task<bool> RemovePurchaseOrder(dynamic DBContext, PurchaseOrder purchaseOrder)
    {
        bool removed = false;
        try
        {
            removed = await RemovePurchaseOrder(DBContext, purchaseOrder);
            return removed;
        }
        catch(Exception ex)
        {
            Log.Error(ex.ToString());
            return removed;
        }
    }

    #endregion
}