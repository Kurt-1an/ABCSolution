using System;
using Serilog;
using ABC.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace ABC.Shared.Services;
public partial class CustomerService_SQL : ComponentBase
{
    #region DICTIONARIES 
    #endregion

    #region FIELDS
    public String AbcDbConnection { get; set; } = String.Empty;
    #endregion

    #region CUSTOMERS CRUD
    public async Task<List<Customer>> GetCustomersList(dynamic DBContext)
    {
        List<Customer> CustomersList = [];
        try
        {
			CustomersList = await GetCustomersListData(DBContext);
            return CustomersList;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return CustomersList;
        }
    }

    public async Task<Customer> GetCustomerInfo(dynamic DBContext, Guid Id)
    {
        Customer CustomerInfo = new();
        try
        {
            CustomerInfo = await GetCustomerData(DBContext, Id);
            return CustomerInfo;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return CustomerInfo;
        }
    }

	// ADD Customer
	public async Task<bool> AddCustomer(dynamic DBContext, Customer customer)
	{
		bool added = false;
		try
		{
			added = await AddCustomerData(DBContext, customer);
			return added;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			return added;
		}
	}

	// UPDATE PRODUCT
	public async Task<bool> UpdateCustomer(dynamic DBContext, Customer customer)
    {
        bool updated = false;
        try
        {
            updated = await UpdateCustomerData(DBContext, customer);
            return updated;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return updated;
        }
    }

    // REMOVE PRODUCT
    public async Task<bool> RemoveCustomer(dynamic DBContext, Customer customer)
    {
        bool removed = false;
        try
        {
            removed = await RemoveCustomerData(DBContext, customer);
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