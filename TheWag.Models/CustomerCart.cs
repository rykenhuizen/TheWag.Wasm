using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWag.Models
{
    public class CustomerCart
    {
        public IList<CartItem> Items { get; init; }
        public Customer? Customer { get; init; }
        public Promo[]? Promo { get; init; }

        public CustomerCart()
        {
            Items = new List<CartItem>();
        }
    }

   
}
