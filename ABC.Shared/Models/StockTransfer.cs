using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Shared.Models;
public class StockTransfer
{
    [Key]
    public int Id { get; set; }
    public DateTime TransferDate { get; set; } = DateTime.UtcNow;
    public DateTime TransferCreationDate { get; set; } = DateTime.UtcNow;
    public int SourceStoreId { get; set; }
    public Store SourceStore { get; set; }
    public int DestinationStoreId { get; set; }
    public Store DestinationStore { get; set; }
    public ApplicationUser applicationUser { get; set; }
    public string Status { get; set; }
    public string TransferRemarks { get; set; }
    public double TransferTotal { get; set; }

    public List<StockTransferItemDetails> StockTransferItems { get; set; }
}
