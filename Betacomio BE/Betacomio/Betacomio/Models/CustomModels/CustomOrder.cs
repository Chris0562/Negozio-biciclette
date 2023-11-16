using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Betacomio.Models.CustomModels
{
    public class CustomOrder
    {
        public DateTime dueDate { get; set; }
        public int customerId { get; set; }
        public int shipAddressId { get; set; }
        public int billToAddressId { get; set; }
        public string shipMethod { get; set; }
        public decimal subTotal { get; set; }
        public CountedCart[] details { get; set; }

    }

    public class CountedCart
    {
        public CustomProduct product { get; set; }
        public int orderQty { get; set; }
    }

    public class CustomProduct
    {
        public int ProductId { get; set; }
        public decimal StandardCost { get; set; }
    }
}


