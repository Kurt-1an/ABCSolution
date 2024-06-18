using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;

public partial class StockTransferService_SQL
{
    #region STOCK TRANSFER CRUD
    //* GET ALL STOCK TRANSFER
    private async Task<List<StockTransfer>> GetStockTransferListData(DbContext DBContext)
    {
        List<StockTransfer> _stockTransferList = [];
        try
        {
            var context = DBContext;
            var stockTransferList = context.Set<StockTransfer>()
                .Include(x => x.StockTransferItems)
                .Include(x => x.applicationUser)
                .Include(x => x.SourceStore)
                .Include(x => x.DestinationStore);
            
            foreach (var item in stockTransferList)
            {
                _stockTransferList.Add(item);
            }
            return _stockTransferList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _stockTransferList;
        }
    }

    //* GETS SINGLE StockTransfer BASE ON PO ID
    private async Task<StockTransfer> GetStockTransferData(DbContext DBContext, int id)
    {
        StockTransfer _stockTransfer = new();
        try
        {
            var context = DBContext;
            var result = await context.Set<StockTransfer>()
                .Include(x => x.StockTransferItems)
                .Include(x => x.applicationUser)
                .Include(x => x.SourceStore)
                .Include(x => x.DestinationStore)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (result is not null)
            {
                _stockTransfer = result;
            }
            return _stockTransfer;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _stockTransfer;
        }
    }

    //* ADDS StockTransfer TO DB
    private async Task<bool> AddStockTransferData(DbContext DBContext, StockTransfer stockTransfer)
    {
        try
        {
            var context = DBContext;
            if (stockTransfer.StockTransferItems != null)
            {
                foreach (var product in stockTransfer.StockTransferItems)
                {
                    context.Entry(product.Product).State = EntityState.Unchanged;
                }
            }
            //if (stockTransfer.SourceStore != null)
            //{
            //    context.Entry(stockTransfer.SourceStore).State = EntityState.Unchanged;
            //}
            context.Set<StockTransfer>().Add(stockTransfer);
            var result = context.SaveChanges();
            return result > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    //* UPDATE stockTransfer ON DB
    private async Task<bool> UpdateStockTransferData(dynamic DBContext, StockTransfer stockTransfer)
    {
        try
        {
            var context = DBContext;
            context.StockTransfers.Update(stockTransfer);
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