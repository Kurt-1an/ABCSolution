using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
		public OrderHeader OrderHeader { get; set; } = new();
        public List<OrderDetail> OrderDetailsList { get; set; } = new();
		public Product Product {  get; set; } = new();

	}
}
