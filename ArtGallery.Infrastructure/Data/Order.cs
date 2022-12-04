using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ArtGallery.Infrastructure.Data.Constants;

namespace ArtGallery.Infrastructure.Data.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public ApplicationUser User { get; set; }

    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    [Required]
    public DateTime TimeOfOrdering { get; set; }

    [Required]
    [MaxLength(ValidationConstants.OrderAddressMaxLength)]
    public string? Address { get; set; }

    public IList<OrderPainting> OrderProducts { get; set; } = new List<OrderPainting>();
}