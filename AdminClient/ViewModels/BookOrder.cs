using System.ComponentModel.DataAnnotations;
using System;

namespace AdminClient.ViewModels
{
    public class BookOrder
    {
        [Key]
        public int BookOrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public int Quantity { get; set; }

        public int ClassId { get; set; }

        public int SchoolId { get; set; }

        public string BillTo { get; set; }       

        public string OrderStatus { get; set; }

        public decimal OrderAmount { get; set; }
    }
}
