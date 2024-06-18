using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Shared.Models
{
    public class StockTransferItemDetails
    {
        [Key]
        public int Id { get; set; }
        public int StockTransferId { get; set; }
        public StockTransfer StockTransfer { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double CostPrice { get; set; }
        public int Quantity { get; set; }
        public double subTotal { get; set; }

    }
}
