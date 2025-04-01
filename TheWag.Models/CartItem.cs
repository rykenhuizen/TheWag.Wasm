using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWag.Models
{
    public class CartItem
    {
        public required ProductDTO Product { get; init; }
        public required int Quantity { get; set; }
    }
}
