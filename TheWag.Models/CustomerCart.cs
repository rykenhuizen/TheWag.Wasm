using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWag.Models
{
    public class CustomerCart
    {
        public IList<CartItem> Items { get; set; }
        public Customer? Customer { get; set; }
        public Promo[]? Promo { get; set; }

        public CustomerCart()
        {
            Items = new List<CartItem>();
        }
    }

   
}
