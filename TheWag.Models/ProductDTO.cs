using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWag.Models
{
    public class ProductDTO
    {
        public int? Id { get; init; }
        public required string URL { get; init; }
        public required decimal Price { get; init; }
        public string PriceCurrency { get => string.Format("{0:C}", Price); }
        public required int Stock { get; init; }
        public required string Description { get; init; }
        public IList<TagDTO> Tags { get; init; } = [];
        public VendorDTO? Vendor { get; init; }
    }
}
