using Serilog;
using Dapper;
using MySql.Data.MySqlClient;
using ABC.Shared.Models;
using System.Net.Sockets;
using ABC.Shared.Models.ViewModels;
using MySqlX.XDevAPI.Common;
using Microsoft.EntityFrameworkCore;

namespace ABC.Shared.Services;

public partial class ShoppingCartService_SQL
{
	#region SHOPPING CART CRUD
	//* GETS ALL SHOPPING CART
	public async Task<List<ShoppingCart>> GetShoppingCartListData(DbContext DBContext, string userId)
	{
		List<ShoppingCart> _shoppingcart = [];

		try
		{
            var products = DBContext.Set<Product>().Include(x => x.StockPerStore);
			var shoppingCarts = DBContext.Set<ShoppingCart>().Where(x => x.ApplicationUserId == userId);

			foreach (var cartItem in shoppingCarts)
			{
				ShoppingCart item = cartItem;
				item.Product = products.FirstOrDefault(p => p.Id == cartItem.ProductId)!;
				_shoppingcart.Add(item);
			}

			return _shoppingcart;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _shoppingcart;
		}
	}

	//* GETS SINGLE CATEGORY BASE ON ID 
	private async Task<ShoppingCart> GetShoppingCartByUserIdAndProductIdData(dynamic DBContext, string userId, int id)
    {
        ShoppingCart shoppingCart = new();

        try
        {
            // Cast lambda expression to delegate 

            Func<ShoppingCart, bool> filter = c =>
            c.ApplicationUserId == userId &&
              c.ProductId == id;


            shoppingCart = await DBContext.ShoppingCarts
              .FirstOrDefaultAsync(filter);

            return shoppingCart;

        }
        catch (Exception ex)
        {

            // Log error
            Log.Error(ex.ToString());
            return shoppingCart;

        }
    }

    //* GETS SINGLE CATEGORY BASE ON ID 
    private async Task<ShoppingCart> GetShoppingCartData(dynamic DBContext, int id)
	{
		ShoppingCart _shoppingcart = new();
		try
		{
			var context = DBContext;
			var result = context.ShoppingCarts.Find(id);
			if (result is not null)
			{
				_shoppingcart = result;
			}
			return _shoppingcart;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return _shoppingcart;
		}
	}

    //* ADDS shopping Cart TO DB
    private async Task<bool> AddShoppingCartData(dynamic DBContext, ShoppingCart shoppingCart)
	{
		try
		{
			var context = DBContext;
			context.ShoppingCarts.Add(shoppingCart);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

    //* UPDATE Shopping Cart ON DB
    private async Task<bool> UpdateShoppingCartData(dynamic DBContext, ShoppingCart shoppingCart)
	{
		try
		{
			var context = DBContext;
			context.ShoppingCarts.Update(shoppingCart);
			var result = context.SaveChanges();
			return result > 0 ? true : false;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return false;
		}
	}

	//* REMOVE/ARCHIVE Shopping Cart FROM DB
	private async Task<bool> RemoveShoppingCartData(dynamic DBContext, ShoppingCart shoppingCart)
	{
		try
		{
			var context = DBContext;
			context.ShoppingCarts.Remove(shoppingCart);
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

