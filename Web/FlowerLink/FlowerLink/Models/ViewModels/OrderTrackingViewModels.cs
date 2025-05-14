using System;
using System.Collections.Generic;

namespace FlowerLink.Models.ViewModels
{
    public class OrderStatusUpdateModel
    {
        public int OrderID { get; set; }
        public int NewStatusID { get; set; }
        public string AdminNotes { get; set; }
    }

    public class OrderStatusViewModel
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string CurrentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
        public List<OrderStatusHistoryViewModel> History { get; set; }
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderStatusHistoryViewModel
    {
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }
    }
}

