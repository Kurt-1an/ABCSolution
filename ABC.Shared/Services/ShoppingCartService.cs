using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class ShoppingCartService_SQL : ComponentBase
{
	#region DICTIONARIES 
	#endregion

	#region FIELDS
	public String AbcDbConnection { get; set; } = String.Empty;


    #endregion

    #region SHOPPING CART CRUD
    public async Task<List<ShoppingCart>> GetShoppingCartList(dynamic DBContext, string userId)
    {
        List<ShoppingCart> ShoppingCartList = [];
        try
        {
            ShoppingCartList = await GetShoppingCartListData(DBContext, userId);
            return ShoppingCartList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return ShoppingCartList;
        }
    }


    public async Task<ShoppingCart> GetShoppingCartByUserIdAndProductId(dynamic DBContext, string userId, int Id)
    {
        ShoppingCart ShoppingCartUserAndProductInfo = new();

        try
        {
            ShoppingCartUserAndProductInfo = await GetShoppingCartByUserIdAndProductIdData(DBContext, userId, Id);
            return ShoppingCartUserAndProductInfo;
        }
        catch (Exception ex)
        {
            // Log error
            Log.Error(ex.ToString());
            return ShoppingCartUserAndProductInfo;
        }
    }

    public async Task<ShoppingCart> GetShoppingCartInfo(dynamic DBContext, int Id)
	{
		ShoppingCart ShoppingCartInfo = new();
		try
		{
			ShoppingCartInfo = await GetShoppingCartData(DBContext, Id);
			return ShoppingCartInfo;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return ShoppingCartInfo;
		}
	}

	// ADD SHOPPING CART
	public async Task<bool> AddShoppingCart(dynamic DBContext, ShoppingCart shoppingCart)
	{
		bool added = false;
		try
		{
			added = await AddShoppingCartData(DBContext, shoppingCart);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

    // UPDATE SHOPPING CART
    public async Task<bool> UpdateShoppingCart(dynamic DBContext, ShoppingCart shoppingCart)
	{
		bool updated = false;
		try
		{
			updated = await UpdateShoppingCartData(DBContext, shoppingCart);
			return updated;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return updated;
		}
	}

	// REMOVE ShoppingCart
	public async Task<bool> RemoveShoppingCart(dynamic DBContext, ShoppingCart shoppingCart)
	{
		bool removed = false;
		try
		{
			removed = await RemoveShoppingCartData(DBContext, shoppingCart);
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

