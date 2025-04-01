using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWag.Models
{
    public record TagDTO
    {
        public int? Id { get; init; }
        public required string Text { get; init; }
    }
}
