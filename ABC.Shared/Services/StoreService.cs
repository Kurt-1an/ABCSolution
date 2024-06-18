using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class StoreService_SQL : ComponentBase
{
	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region STORE CRUD
	public async Task<List<Store>> GetStoreList(dynamic DBContext)
	{
		List<Store> StoreList = [];
		try
		{
			StoreList = await GetStoreListData(DBContext);
			return StoreList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return StoreList;
		}
	}

	public async Task<Store> GetStoreInfo(dynamic DBContext, int Id)
	{
		Store StoreInfo = new();
		try
		{
			StoreInfo = await GetStoreData(DBContext, Id);
			return StoreInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return StoreInfo;
		}
	}

	// ADD store
	public async Task<bool> AddStore(dynamic DBContext, Store store)
	{
		bool added = false;
		try
		{
			added = await AddStoreData(DBContext, store);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE store
	public async Task<bool> UpdateStore(dynamic DBContext, Store store)
	{
		bool updated = false;
		try
		{
			updated = await UpdateStoreData(DBContext, store);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE store
	public async Task<bool> RemoveStore(dynamic DBContext, Store store)
	{
		bool removed = false;
		try
		{
			removed = await RemoveStoreData(DBContext, store);
			return removed;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return removed;
		}
	}
	#endregion
}

