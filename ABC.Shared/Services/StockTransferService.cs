using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;
using ABC.Shared.Utility;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;
public partial class StockTransferService_SQL : ComponentBase
{

    #region FIELDS
    public String AbcDbConnection { get; set; } = String.Empty;
    #endregion

    #region StockTransfer CRUD
    public async Task<StockTransfer> GetStockTransferInfo(DbContext DBContext, int Id)
    {
        StockTransfer StockTransferInfo = new();
        try
        {
            StockTransferInfo = await GetStockTransferData(DBContext, Id);
            return StockTransferInfo;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return StockTransferInfo;
        }
    }

    public async Task<List<StockTransfer>> GetStockTransferList(DbContext DBContext)
    {
        List<StockTransfer> StockTransferList = [];
        try
        {
            StockTransferList = await GetStockTransferListData(DBContext);
            return StockTransferList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return StockTransferList;
        }
    }

    public async Task<bool> AddstockTransfer(DbContext DBContext, StockTransfer stockTransfer)
    {
        bool added = false;
        try
        {
            added = await AddStockTransferData(DBContext, stockTransfer);
            return added;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return added;
        }
    }

    public async Task<bool> UpdateStockTransfer(DbContext DBContext, StockTransfer stockTransfer, ProductService_SQL productService_SQL)
    {
        bool updated = false;
        try
        {
            if (stockTransfer.Status != SD.PO_Rejected)
            {
                foreach (var product in stockTransfer.StockTransferItems)
                {
                    var result2 = await productService_SQL.GetProductInfo(DBContext, product.ProductId);
                    result2.StockPerStore.TotalStocks += product.Quantity;
                    await productService_SQL.UpdateProduct(DBContext, result2, string.Empty);
                }
                updated = await UpdateStockTransferData(DBContext, stockTransfer);
                return updated;
            }
            else
            {
                updated = await UpdateStockTransferData(DBContext, stockTransfer);
                return updated;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return updated;
        }
    }

    #endregion
}