using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;

namespace ABC.Shared.Services;

public partial class CategoryService_SQL
{

	#region CATEGORIES CRUD
	//* GETS ALL CATEGORIES
	public async Task<List<Category>> GetCategoryListData(dynamic DBContext)
	{
		List<Category> _category = [];
		try
		{
			var context = DBContext;
			var categoryList = context.Categories;
			foreach (var item in categoryList)
			{
				_category.Add(item);
			}
			return _category;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _category;
		}
	}

	//* GETS SINGLE CATEGORY BASE ON ID 
	private async Task<Category> GetCategoryData(dynamic DBContext, int id)
	{
		Category _category = new();
		try
		{
			var context = DBContext;
			var result = context.Categories.Find(id);
			if (result is not null)
			{
				_category = result;
			}
			return _category;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _category;
		}
	}

	//* ADDS CATEGORY TO DB
	private async Task<bool> AddCategoryData(dynamic DBContext, Category category)
	{
		try
		{
			var context = DBContext;
			context.Categories.Add(category);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE CATEGORY ON DB
	private async Task<bool> UpdateCategoryData(dynamic DBContext, Category category)
	{
		try
		{
			var context = DBContext;
			context.Categories.Update(category);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE CATEGORY FROM DB
	private async Task<bool> RemoveCategoryData(dynamic DBContext, Category category)
	{
		try
		{
			var context = DBContext;
			context.Categories.Update(category);
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


