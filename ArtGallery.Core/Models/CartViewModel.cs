using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Models
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public IList<CartPaintingViewModel> Paintings { get; set; }
    }
}
