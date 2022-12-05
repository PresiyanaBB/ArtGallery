using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArtGallery.Infrastructure.Data.Constants;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Infrastructure.Data
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(ValidationConstants.ServiceNameMaxLength)]
        public string? Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [MaxLength(ValidationConstants.ServiceDescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
