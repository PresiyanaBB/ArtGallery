using ArtGallery.Infrastructure.Data.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Infrastructure.Data.Models;

public class OrderPainting
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();

    public Order Order { get; set; }

    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }

    public Painting Painting { get; set; }

    [ForeignKey(nameof(Painting))]
    public Guid PaintingId { get; set; }
}