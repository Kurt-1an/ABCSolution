using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;
public partial class ApplicationUserService_SQL : ComponentBase
{
	#region DICTIONARIES 
	#endregion

	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


	#endregion

	#region APPLICATION USER CRUD
	public async Task<List<ApplicationUser>> GetApplicationUserList(dynamic DBContext)
	{
		List<ApplicationUser> ApplicationUserList = [];
		try
		{
			ApplicationUserList = await GetApplicationUserListData(DBContext);
			return ApplicationUserList;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ApplicationUserList;
		}
	}

	public async Task<ApplicationUser> GetApplicationUserInfo(dynamic DBContext, string Id)
	{
		ApplicationUser ApplicationUserInfo = new();
		try
		{
			ApplicationUserInfo = await GetApplicationUserData(DBContext, Id);
			return ApplicationUserInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ApplicationUserInfo;
		}
	}

	// ADD applicationUser
	public async Task<bool> AddApplicationUser(dynamic DBContext, ApplicationUser applicationUser)
	{
		bool added = false;
		try
		{
			added = await AddApplicationUserData(DBContext, applicationUser);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE applicationUser
	public async Task<bool> UpdateApplicationUser(dynamic DBContext, ApplicationUser applicationUser)
	{
		bool updated = false;
		try
		{
			updated = await UpdateApplicationUserData(DBContext, applicationUser);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE applicationUser
	public async Task<bool> RemoveApplicationUser(dynamic DBContext, ApplicationUser applicationUser)
	{
		bool removed = false;
		try
		{
			removed = await RemoveApplicationUserData(DBContext, applicationUser);
			return removed;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return removed;
		}
	}

	public async Task<ApplicationUser> GetCurrentUserInfo(DbContext dbContext, string userId){
		ApplicationUser currentUser = new();
		try
		{
			currentUser = await GetCurrentUserInfoData(dbContext, userId);
			return currentUser;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return currentUser;
		}
	}



	#endregion
}

