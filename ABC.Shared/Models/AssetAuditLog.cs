//using Serilog;

//namespace ABC.Shared.Models
//{
//    public interface IAuditMessage
//    {
//        public string EmailAddress { get; set; }
//        public string Action { get; set; }
//        public string Remarks { get; set; }
//        public DateTime Timestamp { get; set; }
//        public bool IsSuccess { get; set; }

//    }

//    public class ProductAuditLog : IAuditMessage
//    {
//        public string EmailAddress { get; set; } = string.Empty;
//        public string Action { get; set ; } = string.Empty;
//        public string Remarks { get; set; } = string.Empty;
//        public DateTime Timestamp { get; set; } = new();
//        public int ProductId { get; set; } = 0;
//        public string SKU = string.Empty;
//        public bool IsSuccess { get; set; } = false;
//    }

//    //    public static class AddProductMessage
//    //    {
//    //        public const string Success = "Successfully added the product.";

//    //        public const string Failed = "Failed to add the product";
//    //    }

//    //    public static class StockTransferMessage
//    //    {
//    //        public const string Success = "Successfully transferred the stocks.";

//    //        public const string Failed = "Failed to transfer the stocks";
//    //    }
    
//    public static class AuditLogBuilder
//    {
//        public static bool BuildProductAuditLog(this Product product)
//        {
//            ProductAuditLog productAuditLog = new();
//            try
//            {
//                productAuditLog.EmailAddress = 
//                return baseNotification;
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex.ToString());
//                baseNotification.Failed = true;
//                baseNotification.Message = AddProductMessage.Failed;
//                return baseNotification;
//            }
//        }

//        public static AddProductNotice BuildStockTransferNotice(this bool isSuccess)
//        {
//            AddProductNotice baseNotification = new();
//            try
//            {
//                baseNotification.IsSuccess = isSuccess;
//                baseNotification.Failed = !isSuccess;
//                baseNotification.Message = isSuccess ? StockTransferMessage.Success : StockTransferMessage.Failed;
//                return baseNotification;
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex.ToString());
//                baseNotification.Failed = true;
//                baseNotification.Message = StockTransferMessage.Failed;
//                return baseNotification;
//            }
//        }
//    }
//}
