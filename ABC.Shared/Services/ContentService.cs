using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class ContentService_SQL : ComponentBase
{
	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region CONTENT CRUD
	public async Task<List<Content>> GetContentList(dynamic DBContext)
	{
		List<Content> ContentList = [];
		try
		{
			ContentList = await GetContentListData(DBContext);
			return ContentList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ContentList;
		}
	}

	public async Task<Content> GetContentInfo(dynamic DBContext, int Id)
	{
		Content ContentInfo = new();
		try
		{
			ContentInfo = await GetContentData(DBContext, Id);
			return ContentInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ContentInfo;
		}
	}

	// ADD CONTENT
	public async Task<bool> AddContent(dynamic DBContext, Content content)
	{
		bool added = false;
		try
		{
			added = await AddContentData(DBContext, content);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE CONTENT
	public async Task<bool> UpdateContent(dynamic DBContext, Content content)
	{
		bool updated = false;
		try
		{
			updated = await UpdateContentData(DBContext, content);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE CONTENT
	public async Task<bool> RemoveContent(dynamic DBContext, Content content)
	{
		bool removed = false;
		try
		{
			removed = await RemoveContentData(DBContext, content);
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

