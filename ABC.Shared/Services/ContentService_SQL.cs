using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;

namespace ABC.Shared.Services;
public partial class ContentService_SQL
{
	#region CONTENT CRUD
	//* GETS ALL CONTENTS
	public async Task<List<Content>> GetContentListData(dynamic DBContext)
	{
		List<Content> _content = [];
		try
		{
			var context = DBContext;
			var contentsList = context.Contents;
			foreach (var item in contentsList)
			{
				_content.Add(item);
			}
			return _content;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _content;
		}
	}

	//* GETS SINGLE CONTENT BASE ON ID 
	private async Task<Content> GetContentData(dynamic DBContext, int id)
	{
		Content _content = new();
		try
		{
			var context = DBContext;
			var result = context.Contents.Find(id);
			if (result is not null)
			{
				_content = result;
			}
			return _content;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _content;
		}
	}

	//* ADDS CONTENT TO DB
	private async Task<bool> AddContentData(dynamic DBContext, Content content)
	{
		try
		{
			var context = DBContext;
			context.Contents.Add(content);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE CONTENT ON DB
	private async Task<bool> UpdateContentData(dynamic DBContext, Content content)
	{
		try
		{
			var context = DBContext;
			context.Contents.Update(content);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE CONTENT FROM DB
	private async Task<bool> RemoveContentData(dynamic DBContext, Content content)
	{
		try
		{
			var context = DBContext;
			context.Contents.Update(content);
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

