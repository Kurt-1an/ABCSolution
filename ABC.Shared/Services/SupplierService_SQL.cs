using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;
public partial class SupplierService_SQL
{
	#region SUPPLIER CRUD
	//* GETS ALL SUPPLIERS
	public async Task<List<Supplier>> GetSupplierListData(dynamic DBContext)
	{
		List<Supplier> _supplier = [];
		try
		{
			var context = DBContext;
			var suppliersList = context.Suppliers;
			foreach (var item in suppliersList)
			{
				_supplier.Add(item);
			}
			return _supplier;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _supplier;
		}
	}

	//* GETS SINGLE SUPPLIER BASE ON ID 
	private async Task<Supplier> GetSupplierData(dynamic DBContext, int id)
	{
		Supplier _supplier = new();
		try
		{
			var context = DBContext;
			var result = context.Suppliers.Find(id);
			if (result is not null)
			{
				_supplier = result;
			}
			return _supplier;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _supplier;
		}
	}

	//* ADDS SUPPLIER TO DB
	private async Task<bool> AddSupplierData(DbContext DBContext, Supplier supplier, string employeeId)
	{
		try
		{
            DBContext.Set<Supplier>().Add(supplier);
			var result = DBContext.SaveChanges() > 0;

			supplier.SupplierAudit(AuditActions.AddSupplier, employeeId, result, DBContext);

			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE SUPPLIER ON DB
	private async Task<bool> UpdateSupplierData(DbContext DBContext, Supplier supplier, string employeeId)
	{
		try
		{
            DBContext.Set<Supplier>().Update(supplier);
            var result = DBContext.SaveChanges() > 0;

            supplier.SupplierAudit(AuditActions.UpdateSupplier, employeeId, result, DBContext);
            return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* ARCHIVE SUPPLIER FROM DB
	private async Task<bool> ArchiveSupplierData(DbContext DBContext, Supplier supplier, string employeeId)
	{
		try
		{
            DBContext.Set<Supplier>().Update(supplier);
            var result = DBContext.SaveChanges() > 0;

            supplier.SupplierAudit(AuditActions.ArchiveSupplier, employeeId, result, DBContext);
            return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

    public async Task<bool> ReactivateSupplierData(DbContext DBContext, Supplier supplier, string employeeId)
    {
        try
        {
            DBContext.Set<Supplier>().Update(supplier);
            var result = DBContext.SaveChanges() > 0;

            supplier.SupplierAudit(AuditActions.ReactivateSupplier, employeeId, result, DBContext);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return false;
        }
    }

    #endregion
}

