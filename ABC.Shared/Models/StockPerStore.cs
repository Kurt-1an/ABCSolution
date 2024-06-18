using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Shared.Models;
public class StockPerStore
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Store1StockQty { get; set; } = 0;
    public int MinimumStore1StockQty { get; set; } = 1;
    public int Store2StockQty { get; set; } = 0;
    public int MinimumStore2StockQty { get; set; } = 1;
    public int TotalStocks { get; set; } = 0;
}
