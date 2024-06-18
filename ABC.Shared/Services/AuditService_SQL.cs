using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;

public partial class AuditService_SQL
{
    //* GET ALL Audit
    private async Task<List<AuditLog>> GetAuditsListData(dynamic DBContext)
    {
        List<AuditLog> _auditList = [];
        try
        {
            var context = DBContext;
            var auditList = context.AuditLogs;
            foreach (var item in auditList)
            {
                _auditList.Add(item);
            }
            return _auditList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _auditList;
        }
    }

    //* GETS SINGLE Audit BASE ON Audit ID
    private async Task<AuditLog> GetAuditData(dynamic DBContext, int id)
    {
        AuditLog _audit = new();
        try
        {
            var context = DBContext;
            var result = context.AuditLogs.Find(id);
            if (result is not null)
            {
                _audit = result;
            }
            return _audit;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _audit;
        }
    }

    //* ADDS Audit TO DB
    private async Task<bool> AddAuditData(dynamic DBContext, AuditLog auditLog)
    {
        try
        {
            var context = DBContext;
            context.AuditLogs.Add(auditLog);
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    private async Task<List<StockTransferAudit>> GetStockAuditsListData(DbContext DBContext)
    {
        List<StockTransferAudit> _stockauditList = [];
        try
        {
            var auditList = DBContext.Set<StockTransferAudit>()
                .Include(x => x.StockPerStore).ThenInclude(x => x.Product);
			foreach (var item in auditList)
            {
                _stockauditList.Add(item);
            }
            return _stockauditList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _stockauditList;
        }
    }

	private async Task<List<ProductAudit>> GetProductAuditsListData(DbContext DBContext)
	{
		List<ProductAudit> _productAuditList = [];
		try
		{
			var auditList = DBContext.Set<ProductAudit>().Include(x => x.Product);
			foreach (var item in auditList)
			{
				_productAuditList.Add(item);
			}
			return _productAuditList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _productAuditList;
		}
	}

    private async Task<List<SupplierAudit>> GetSupplierAuditsListData(DbContext DBContext)
    {
        List<SupplierAudit> _supplierAuditList = [];
        try
        {
            var auditList = DBContext.Set<SupplierAudit>().Include(x => x.Supplier);
            foreach (var item in auditList)
            {
                _supplierAuditList.Add(item);
            }
            return _supplierAuditList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _supplierAuditList;
        }
    }

    private async Task<bool> AddStockTransferAuditData(DbContext DBContext, StockTransferAudit auditLog)
    {
        try
        {
            DBContext.Set<StockTransferAudit>().Add(auditLog);
            var result = DBContext.SaveChanges();
            return result > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    private async Task<bool> AddProductAuditData(DbContext DBContext, ProductAudit auditLog)
    {
        try
        {
            DBContext.Set<ProductAudit>().Add(auditLog);
            var result = DBContext.SaveChanges();
            return result > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    private async Task<bool> AddSupplierAuditData(DbContext DBContext, SupplierAudit auditLog)
    {
        try
        {
            DBContext.Set<SupplierAudit>().Add(auditLog);
            var result = DBContext.SaveChanges();
            return result > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }
}