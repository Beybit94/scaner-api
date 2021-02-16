using System.Collections.Generic;

namespace Business.Models
{
    public class DifferencesModel
    {
        public List<GoodsModel> boxes = new List<GoodsModel>();
        public HashSet<ReceiptModel> receipts = new HashSet<ReceiptModel>();
    }
}
