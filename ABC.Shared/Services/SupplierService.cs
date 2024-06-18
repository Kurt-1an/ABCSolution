using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class SupplierService_SQL : ComponentBase
{
	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region SUPPLIER CRUD
	public async Task<List<Supplier>> GetSupplierList(dynamic DBContext)
	{
		List<Supplier> SupplierList = [];
		try
		{
			SupplierList = await GetSupplierListData(DBContext);
			return SupplierList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return SupplierList;
		}
	}

	public async Task<Supplier> GetSupplierInfo(dynamic DBContext, int Id)
	{
		Supplier SupplierInfo = new();
		try
		{
			SupplierInfo = await GetSupplierData(DBContext, Id);
			return SupplierInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return SupplierInfo;
		}
	}

	// ADD SUPPLIER
	public async Task<bool> AddSupplier(dynamic DBContext, Supplier supplier, string employeeName)
	{
		bool added = false;
		try
		{
			added = await AddSupplierData(DBContext, supplier, employeeName);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE SUPPLIER
	public async Task<bool> UpdateSupplier(dynamic DBContext, Supplier supplier, string employeeName)
	{
		bool updated = false;
		try
		{
			updated = await UpdateSupplierData(DBContext, supplier, employeeName);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	public async Task<bool> ArchiveSupplier(dynamic DBContext, Supplier supplier, string employeeName)
	{
		bool removed = false;
		try
		{
			removed = await ArchiveSupplierData(DBContext, supplier, employeeName);
			return removed;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return removed;
		}
	}

    public async Task<bool> ReactivateSupplier(dynamic DBContext, Supplier supplier, string employeeName)
    {
        bool reactivated = false;
        try
        {
            supplier.supplierStatus = "Active";

            reactivated = await ReactivateSupplierData(DBContext, supplier, employeeName);
            return reactivated;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return reactivated;
        }
    }
    #endregion
}

