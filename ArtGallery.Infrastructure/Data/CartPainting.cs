using ArtGallery.Infrastructure.Data.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Infrastructure.Data.Models;

public class CartPainting
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Cart Cart { get; set; }

    [ForeignKey(nameof(Cart))]
    public Guid CartId { get; set; }

    public Painting Painting { get; set; }

    [ForeignKey(nameof(Painting))]
    public Guid PaintingId { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool Ordered { get; set; }
}