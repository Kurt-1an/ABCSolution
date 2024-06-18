using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;

namespace ABC.Shared.Services;
public partial class StoreService_SQL
{
	#region SUPPLIER CRUD
	//* GETS ALL STORES
	public async Task<List<Store>> GetStoreListData(dynamic DBContext)
	{
		List<Store> _store = [];
		try
		{
			var context = DBContext;
			var storesList = context.Stores;
			foreach (var item in storesList)
			{
				_store.Add(item);
			}
			return _store;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _store;
		}
	}

	//* GETS SINGLE STORE BASE ON ID 
	private async Task<Store> GetStoreData(dynamic DBContext, int id)
	{
		Store _store = new();
		try
		{
			var context = DBContext;
			var result = context.Stores.Find(id);
			if (result is not null)
			{
				_store = result;
			}
			return _store;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _store;
		}
	}

	//* ADDS STORE TO DB
	private async Task<bool> AddStoreData(dynamic DBContext, Store store)
	{
		try
		{
			var context = DBContext;
			context.Stores.Add(store);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE STORE ON DB
	private async Task<bool> UpdateStoreData(dynamic DBContext, Store store)
	{
		try
		{
			var context = DBContext;
			context.Stores.Update(store);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE STORE FROM DB
	private async Task<bool> RemoveStoreData(dynamic DBContext, Store store)
	{
		try
		{
			var context = DBContext;
			context.Stores.Update(store);
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

