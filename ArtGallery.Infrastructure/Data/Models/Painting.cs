using ArtGallery.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Infrastructure.Data.Models;

public class Painting
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(ValidationConstants.PaintingNameMaxLength)]
    public string? Name { get; set; }

    [MaxLength(ValidationConstants.PaintingDescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ImagePathMaxLength)]
    public string? ImagePath { get; set; }

    public bool isAvailable { get; set; } = true;

    [Required]
    public decimal Price { get; set; }

    public Category Category { get; set; }

    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }

    public IList<OrderPainting> OrderProducts { get; set; } = new List<OrderPainting>();
    public IList<CartPainting> CartPainting { get; set; } = new List<CartPainting>();
}