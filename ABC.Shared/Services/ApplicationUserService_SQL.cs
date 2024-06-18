using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ABC.Shared.Services;

public partial class ApplicationUserService_SQL
{
	#region ApplicationUsers CRUD
	//* GETS ALL ApplicationUsers
	public async Task<List<ApplicationUser>> GetApplicationUserListData(DbContext context)
	{
		List<ApplicationUser> _applicationUser = [];
		try
		{
			var applicationUserList = await context.Set<ApplicationUser>().ToListAsync();
			var userRoles = await context.Set<IdentityUserRole<string>>().ToListAsync();
			var roles = await context.Set<IdentityRole>().ToListAsync();
			//IEnumerable<Store> stores = context.Stores;
			foreach (var item in applicationUserList)
			{
				var userRoleIds = userRoles.FirstOrDefault(x => x.UserId == item.Id);
				if (userRoleIds != null)
				{
					var userRole = roles.FirstOrDefault(x => x.Id == userRoleIds.RoleId);
					if (userRole != null)
					{
						item.Role = userRole.Name;
					}
				}
				_applicationUser.Add(item);
			}
			return _applicationUser;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _applicationUser;
		}
	}

	//* GETS SINGLE ApplicationUsers BASE ON ID 
	private async Task<ApplicationUser> GetApplicationUserData(DbContext DBContext, string id)
	{
		ApplicationUser _applicationUser = new();
		try
		{
			ApplicationUser result = DBContext.Set<ApplicationUser>().FirstOrDefault(x => x.Id == id);

			if (result is not null)
			{
				_applicationUser = result;
			}
			return _applicationUser;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _applicationUser;
		}
	}

	//* GETS CURRENT USER INFORMATION
	private async Task<ApplicationUser> GetCurrentUserInfoData(DbContext DBContext, string userId){
		ApplicationUser currentUser = new();
		try
		{
			currentUser = DBContext.Set<ApplicationUser>().FirstOrDefault( x => x.Id == userId);
			return currentUser;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return currentUser;			
		}
	}

	//* ADDS ApplicationUsers TO DB
	private async Task<bool> AddApplicationUserData(dynamic DBContext, ApplicationUser applicationUser)
	{
		try
		{
			var context = DBContext;
			context.ApplicationUsers.Add(applicationUser);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* UPDATE ApplicationUsers ON DB
	private async Task<bool> UpdateApplicationUserData(dynamic DBContext, ApplicationUser applicationUser)
	{
		try
		{
			var context = DBContext;
			context.ApplicationUsers.Update(applicationUser);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE ApplicationUsers FROM DB
	private async Task<bool> RemoveApplicationUserData(dynamic DBContext, ApplicationUser applicationUser)
	{
		try
		{
			var context = DBContext;
			context.ApplicationUsers.Update(applicationUser);
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

