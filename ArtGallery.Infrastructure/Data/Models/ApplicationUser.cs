using ArtGallery.Infrastructure.Data.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(ValidationConstants.UserNamesMaxLength)]
        public string UserName { get; set; }

        public Cart Cart { get; set; }
        [ForeignKey(nameof(Cart))]
        public Guid CartId { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
