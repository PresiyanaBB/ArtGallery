using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Models
{
    public class StorePageViewModel
    {
        public int PageNumber { get; set; }
        public int PagesCount { get; set; }
        public IEnumerable<PaintingViewModel> Paintings { get; set; } = new List<PaintingViewModel>();
    }
}
