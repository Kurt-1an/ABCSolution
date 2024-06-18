using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class CategoryService_SQL : ComponentBase
{
	#region DICTIONARIES 
	#endregion

	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region CATEGORIES CRUD
	public async Task<List<Category>> GetCategoryList(dynamic DBContext)
	{
		List<Category> CategoryList = [];
		try
		{
			CategoryList = await GetCategoryListData(DBContext);
			return CategoryList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return CategoryList;
		}
	}

	public async Task<Category> GetCategoryInfo(dynamic DBContext, int Id)
	{
		Category CategoryInfo = new();
		try
		{
			CategoryInfo = await GetCategoryData(DBContext, Id);
			return CategoryInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return CategoryInfo;
		}
	}

	// ADD CATEGORY
	public async Task<bool> AddCategory(dynamic DBContext, Category category)
	{
		bool added = false;
		try
		{
			added = await AddCategoryData(DBContext, category);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE CATEGORY
	public async Task<bool> UpdateCategory(dynamic DBContext, Category category)
	{
		bool updated = false;
		try
		{
			updated = await UpdateCategoryData(DBContext, category);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE CATEGORY
	public async Task<bool> RemoveCategory(dynamic DBContext, Category category)
	{
		bool removed = false;
		try
		{
			removed = await RemoveCategoryData(DBContext, category);
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


