using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ABC.Shared.Services;

namespace ABC.Shared.Models;
public interface IAudit
{
    public int Id { get; set; }
    public string Action { get; set; }
    public bool IsSuccess { get; set; }
    public bool Failed { get; set; }
    public string EmployeeName { get; set; }
    public DateTime Timestamp { get; set; }

}

public class ProductAudit : IAudit
{
    public int Id { get; set; } = 0;
    public string Action { get; set; } = String.Empty;
    public bool IsSuccess { get; set; } = false;
    public bool Failed { get; set; } = false;
    public string EmployeeName { get; set; } = String.Empty;
    public DateTime Timestamp { get; set; } = new();

    //Product Specific
    public int ProductId { get; set; } = 0;
    public Product Product { get; set; } = new();
}

public class StockTransferAudit : IAudit
{
    public int Id { get; set; } = 0;
    public string Action { get; set; } = String.Empty;
    public bool IsSuccess { get; set; } = false;
    public bool Failed { get; set; } = false;
    public string EmployeeName { get; set; } = String.Empty;
    public DateTime Timestamp { get; set; } = new();

    // STOCK TRANSFER SPECIFIC
    public int StockPerStoreId { get; set; } = 0;
    public StockPerStore StockPerStore { get; set; }
    public string SourceStoreName { get; set; } = String.Empty;
    public string DescitnationStoreName { get; set; } = String.Empty;
    public int TransferredStocks { get; set; }= 0;
}

public class SupplierAudit : IAudit
{
    public int Id { get; set; } = 0;
    public string Action { get; set; } = String.Empty;
    public bool IsSuccess { get; set; } = false;
    public bool Failed { get; set; } = false;
    public string EmployeeName { get; set; } = String.Empty;
    public DateTime Timestamp { get; set; } = new();

    public int SupplierId { get; set; } = 0;
    public Supplier Supplier { get; set; } = new();
}

public static class AuditActions
{
    public const string TransferStock = "Stock Transfer";

    public const string AddProduct = "Add Product";
    public const string UpdateProduct = "Update Product";
    public const string RemoveProduct = "Remove Product";

    public const string AddSupplier = "Add Supplier";
    public const string UpdateSupplier = "Update Supplier";
    public const string ArchiveSupplier = "Archive Supplier";
    public const string ReactivateSupplier = "Reactivate Supplier";
}

public static class AuditLogBuilder
{
    public static async Task CreateProductTransferAudit(this StockTransferAudit stockPerStore, string employeeName, bool isSuccess, StockTransferAudit stockTransfer, DbContext DBContext)
    {
        try
        {
            StockTransferAudit baseAudit = stockPerStore;
            AuditService_SQL auditService = new();
            if(baseAudit is not null){
                bool hasAdded = await auditService.AddStockTransferAudit(DBContext, baseAudit);
            }
            // POSTING THE AUDIT TO STOCK TRANSFER AUDIT TABLE
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }

	
	public static async Task ProductAudit(this Product product, string action, string employeeName, bool isSuccess, DbContext DBContext)
    {
        try
        {
            ProductAudit baseAudit = new()
            {
                Action = action,
                IsSuccess = isSuccess,
                Failed = !isSuccess,
                EmployeeName = employeeName,
                Timestamp = DateTime.UtcNow,
                ProductId = product.Id,
                Product = product,
            };
            AuditService_SQL auditService = new();
            if(baseAudit is not null)
            {
                bool hasAdded = await auditService.AddProductsAudit(DBContext, baseAudit);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }

    public static async Task SupplierAudit(this Supplier supplier, string action, string employeeName, bool isSuccess, DbContext DBContext)
    {
        try
        {
            SupplierAudit baseAudit = new()
            {
                Action = action,
                IsSuccess = isSuccess,
                Failed = !isSuccess,
                EmployeeName = employeeName,
                Timestamp = DateTime.UtcNow,
                SupplierId = supplier.Id,
                Supplier = supplier,
            };
            AuditService_SQL auditService = new();
            if (baseAudit is not null)
            {
                bool hasAdded = await auditService.AddSupplierAudit(DBContext, baseAudit);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }
}


