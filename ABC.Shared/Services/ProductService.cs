using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;
public partial class ProductService_SQL : ComponentBase
{
	#region DICTIONARIES 
	#endregion

	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region PRODUCTS CRUD
	public async Task<Product> GetProductInfo(DbContext DBContext, int Id)
	{
		Product ProductInfo = new();
		try
		{
			ProductInfo = await GetProductData(DBContext, Id);
			return ProductInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ProductInfo;
		}
	}

	public async Task<List<Product>> GetProductList(DbContext DBContext)
	{
		List<Product> ProductList = [];
		try
		{
			ProductList = await GetProductsListData(DBContext);
			return ProductList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ProductList;
		}
	}

	// ADD PRODUCT
	public async Task<bool> AddProduct(dynamic DBContext, Product product, StockPerStore stockPerStore, string employeeName)
	{
		bool added = false;
		try
		{
			added = await AddProductData(DBContext, product, stockPerStore, employeeName);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE PRODUCT
	public async Task<bool> UpdateProduct(dynamic DBContext, Product product, string employeeName)
	{
		bool updated = false;
		try
		{
			updated = await UpdateProductData(DBContext, product, employeeName);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE PRODUCT
	public async Task<bool> RemoveProduct(dynamic DBContext, Product product, string employeeName)
	{
		bool removed = false;
		try
		{
			removed = await RemoveProductData(DBContext, product, employeeName);
			return removed;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return removed;
		}
	}

    #endregion

    public async Task<bool> TransferStock(DbContext dbContext, StockPerStore stockPerStore, string employeeName)
    {
        bool stockTransferUpdated = false;
        try
        {
            stockTransferUpdated = await UpdateStockPerStoreData(dbContext, stockPerStore, employeeName);
            return stockTransferUpdated;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return stockTransferUpdated;
        }
    }
    public async Task<StockPerStore> GetStockperStoreInfo(DbContext DBContext, int Id)
    {
        StockPerStore StockperStoreInfo = new();
        try
        {
            StockperStoreInfo = await GetStockperStoreInfoData(DBContext, Id);
            return StockperStoreInfo;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return StockperStoreInfo;
        }
    }
    public async Task<bool> UpdateStockPerStore(DbContext dbContext, StockPerStore stockPerStore, string employeeName)
    {
        bool stockTransferUpdated = false;
        try
        {
            stockTransferUpdated = await UpdateStockPerStoreData(dbContext, stockPerStore, employeeName);
            return stockTransferUpdated;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return stockTransferUpdated;
        }
    }

    public async Task<List<StockPerStore>> GetStockPerStoreList(DbContext DBContext)
    {
        List<StockPerStore> stockList = [];
        try
        {
            stockList = await GetStockperStoreListData(DBContext);
            return stockList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return stockList;
        }
    }
}