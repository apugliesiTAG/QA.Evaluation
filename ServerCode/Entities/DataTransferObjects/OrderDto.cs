using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public CustomerDto Customer { get; set; }
        public string Employee { get; set; }
        public int EmployeeId { get; set; }
        public decimal Freight { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
        public string ShipName { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipRegion { get; set; }
        public int ShipVia { get; set; }
        public DateTime? ShippedDate { get; set; }
        public ShipperDto Shipper { get; set; }
    }
}
