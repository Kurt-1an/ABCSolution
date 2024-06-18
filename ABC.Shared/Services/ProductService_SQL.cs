using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using ABC.Shared.Models.ViewModels;

namespace ABC.Shared.Services;

public partial class ProductService_SQL
{
	#region PRODUCTS CRUD
	//* GET ALL PRODUCTS
	private async Task<List<Product>> GetProductsListData(DbContext DBContext)
	{
		List<Product> _product = [];
		try
		{
			var result = DBContext.Set<Product>().Include(x => x.StockPerStore);
            _product = result.ToList();
            return _product;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _product;
		}
	}

	//* GETS SINGLE PRODUCT BASE ON PRODUCT ID
	private async Task<Product> GetProductData(DbContext DBContext, int id)
	{
		Product _product = new();
		try
		{
			var context = DBContext;
			var result = context.Set<Product>().Include(x => x.StockPerStore).FirstOrDefault( x => x.Id == id);
			if (result is not null)
			{
				_product = result;
			}
			return _product;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _product;
		}
	}

	//* ADDS PRODUCT TO DB
	private async Task<bool> AddProductData(DbContext DBContext, Product product, StockPerStore stockPerStore, string employeeId)
	{
		try
		{
			DBContext.Set<Product>().Add(product);
			var result = DBContext.SaveChanges() > 0;
			
			stockPerStore.Product = product;
			DBContext.Set<StockPerStore>().Add(stockPerStore);
			var result2 = DBContext.SaveChanges() > 0;

			product.ProductAudit(AuditActions.AddProduct, employeeId, result, DBContext);

			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE PRODUCT ON DB
	private async Task<bool> UpdateProductData(DbContext DBContext, Product product, string employeeId)
	{
		bool updated = false;
		try
		{
			DBContext.Set<Product>().Update(product);
			var result = DBContext.SaveChanges() > 0;

			if (!String.IsNullOrEmpty(employeeId))
			{
                product.ProductAudit(AuditActions.UpdateProduct, employeeId, result, DBContext);
            }

			return updated = true;
        }
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE PRODUCT FROM DB
	private async Task<bool> RemoveProductData(DbContext DBContext, Product product, string employeeId)
	{
		try
		{
			DBContext.Set<Product>().Update(product);
			var result = DBContext.SaveChanges() > 0;

			if (result == true)
			{
                product.ProductAudit(AuditActions.RemoveProduct, employeeId, result, DBContext);
            }
			
			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}
    #endregion

    private async Task<bool> UpdateStockPerStoreData(DbContext DBContext, StockPerStore stockPerStore, string employeeName)
    {
        bool updatedStockPerStore = false;
        try
        {
			stockPerStore.TotalStocks = stockPerStore.Store1StockQty + stockPerStore.Store2StockQty;
            var stores = DBContext.Set<Store>();
            var products = DBContext.Set<Product>();
            DBContext.Set<StockPerStore>().Update(stockPerStore);
            DBContext.ChangeTracker.DetectChanges();
            var test = DBContext.ChangeTracker.DebugView.LongView;
            var modifiedEntities = DBContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            Console.WriteLine(DBContext.ChangeTracker.DebugView.LongView);

            StockTransferAudit audit = new()
            {
                Action = AuditActions.TransferStock,
                EmployeeName = employeeName,
                Timestamp = DateTime.UtcNow.ToLocalTime(),
                StockPerStoreId = stockPerStore.Id,
            };

            foreach (var entry in DBContext.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {
                    var originalValues = entry.OriginalValues;
                    var currentValues = entry.CurrentValues;

                    // Iterate through the properties to find modified ones
                    foreach (var property in originalValues.Properties)
                    {
						if (property.Name.Contains("RefundedBy"))
						{
							Console.WriteLine("error");
						}
                        var originalValue = originalValues[property];
                        var currentValue = currentValues[property];


                        // Check if the value of the property has changed
                        if (!Equals(originalValue, currentValue))
                        {
                            var propertyName = property.Name;
                            int storeId = propertyName.Equals("Store1StockQty") ? 1 : 2;
                            var entityType = entry.Entity.GetType().Name;
							originalValue ??= 0;
							currentValue ??= 0;
							if (int.TryParse(originalValue.ToString(), out int _originalValue) && int.TryParse(currentValue.ToString(), out int _currentValue))
							{
								if (_originalValue > _currentValue)
								{
									audit.TransferredStocks = Convert.ToInt32(originalValue) - Convert.ToInt32(currentValue);
									audit.SourceStoreName = stores.FirstOrDefault(x => x.Id == storeId)!.storeName;
									audit.DescitnationStoreName = stores.FirstOrDefault(x => x.Id != storeId)!.storeName;
									break;
								}
							}
                        }
                    }
                }
            }

            var result = await DBContext.SaveChangesAsync() > 0;
            audit.IsSuccess = result;
            audit.Failed = !result;

            await audit.CreateProductTransferAudit(employeeName, result, audit, DBContext);
            return updatedStockPerStore = result;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return updatedStockPerStore;
        }
    }

    //* GETS SINGLE StockperStore BASE ON PRODUCT ID
    private async Task<StockPerStore> GetStockperStoreInfoData(DbContext DBContext, int id)
    {
        StockPerStore _stockPerStore = new();
        try
        {
            var context = DBContext;
            var result = context.Set<StockPerStore>().FirstOrDefault(x => x.Id == id);
            if (result is not null)
            {
                _stockPerStore = result;
            }
            return _stockPerStore;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _stockPerStore;
        }
    }

    //* GETS List of StockperStore BASE ON PRODUCT ID
    private async Task<List<StockPerStore>> GetStockperStoreListData(DbContext DBContext)
    {
        List<StockPerStore> _stockPerStore = [];
        try
        {
            var result = DBContext.Set<StockPerStore>().ToList();
            if (result is not null)
            {
                _stockPerStore = result;
            }
            return _stockPerStore;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return _stockPerStore;
        }
    }
}