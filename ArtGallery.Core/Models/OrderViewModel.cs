using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Models
{
    public class OrderViewModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public DateTime OrderTime { get; set; }
        public Status Status { get; set; }
        public IList<OrderPaintingViewModel> Paintings { get; set; } = new List<OrderPaintingViewModel>();
    }
}
